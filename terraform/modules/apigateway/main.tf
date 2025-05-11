resource "aws_api_gateway_rest_api" "orders_api" {
  name        = "OrdersAPI"
  description = "API Gateway for orders service"
}

# resource "aws_iam_role" "apigateway_logging_role" {
#   name = "APIGatewayLoggingRole"

#   assume_role_policy = jsonencode({
#     Version = "2012-10-17"
#     Statement = [
#       {
#         Effect = "Allow"
#         Principal = {
#           Service = "apigateway.amazonaws.com"
#         }
#         Action = "sts:AssumeRole"
#       }
#     ]
#   })
# }

# resource "aws_iam_policy" "apigateway_logging_policy" {
#   name        = "APIGatewayLoggingPolicy"
#   description = "Policy to allow API Gateway logging to CloudWatch"

#   policy = jsonencode({
#     Version = "2012-10-17"
#     Statement = [
#       {
#         Effect = "Allow"
#         Action = [
#           "logs:CreateLogGroup",
#           "logs:CreateLogStream",
#           "logs:DescribeLogGroups",
#           "logs:DescribeLogStreams",
#           "logs:PutLogEvents"
#         ]
#         Resource = "arn:aws:logs:${var.aws_region}:${var.aws_account_id}:log-group:/aws/apigateway/orders-api:*"
#       }
#     ]
#   })
# }

# resource "aws_iam_role_policy_attachment" "attach_logging_policy" {
#   role       = aws_iam_role.apigateway_logging_role.name
#   policy_arn = aws_iam_policy.apigateway_logging_policy.arn
# }

# resource "aws_api_gateway_account" "gateway_account_settings" {
#   cloudwatch_role_arn = aws_iam_role.apigateway_logging_role.arn
# }

resource "aws_api_gateway_rest_api_policy" "orders_api_policy" {
  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect    = "Allow"
        Principal = "*"
        Action    = "execute-api:Invoke"
        Resource  = "${aws_api_gateway_rest_api.orders_api.execution_arn}/*/*"
      },
      {
        Effect = "Allow"
        Principal = {
          "Service" : "apigateway.amazonaws.com"
        }
        Action   = "lambda:InvokeFunction"
        Resource = "arn:aws:lambda:${var.aws_region}:${var.aws_account_id}:function:*"
      }
    ]
  })
}

resource "aws_lambda_permission" "api_gateway_invoke" {
  for_each = var.api_gateway_lambda_definitions

  function_name = each.value.lambda_name
  statement_id  = "AllowExecutionFromAPIGateway-${each.key}"
  action        = "lambda:InvokeFunction"
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_api_gateway_rest_api.orders_api.execution_arn}/*/*"
}

resource "aws_api_gateway_resource" "orders" {
  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  parent_id   = aws_api_gateway_rest_api.orders_api.root_resource_id
  path_part   = "orders"
}

resource "aws_api_gateway_method" "options_method" {
  rest_api_id   = aws_api_gateway_rest_api.orders_api.id
  resource_id   = aws_api_gateway_resource.orders.id
  http_method   = "OPTIONS"
  authorization = "NONE"
}

resource "aws_api_gateway_method_response" "options_response" {
  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  resource_id = aws_api_gateway_resource.orders.id
  http_method = "OPTIONS"
  status_code = "200"
  depends_on = [aws_api_gateway_method.options_method]

  response_parameters = {
    "method.response.header.Access-Control-Allow-Origin"  = true
    "method.response.header.Access-Control-Allow-Methods" = true
    "method.response.header.Access-Control-Allow-Headers" = true
  }
}

resource "aws_api_gateway_integration" "options_integration" {
  rest_api_id             = aws_api_gateway_rest_api.orders_api.id
  resource_id             = aws_api_gateway_resource.orders.id
  http_method             = "OPTIONS"
  integration_http_method = "OPTIONS"
  type                    = "MOCK"
  depends_on              = [aws_api_gateway_method.options_method]

  request_templates = {
    "application/json" = jsonencode(
      {
        statusCode = 200
      }
    )
  }
}

resource "aws_api_gateway_integration_response" "options_integration_response" {
  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  resource_id = aws_api_gateway_resource.orders.id
  depends_on = [aws_api_gateway_integration.options_integration]
  http_method = "OPTIONS"
  status_code = "200"

  response_parameters = {
    "method.response.header.Access-Control-Allow-Origin"  = "'*'"
    "method.response.header.Access-Control-Allow-Methods" = "'OPTIONS,GET,POST, PUT,DELETE'"
    "method.response.header.Access-Control-Allow-Headers" = "'Content-Type'"
  }
}

resource "aws_api_gateway_method" "orders_method" {
  for_each    = var.api_gateway_lambda_definitions
  resource_id = aws_api_gateway_resource.orders.id

  rest_api_id   = aws_api_gateway_rest_api.orders_api.id
  http_method   = each.value.http_method
  authorization = "NONE"
}

resource "aws_api_gateway_method_response" "orders_cors_response" {
  for_each = var.api_gateway_lambda_definitions

  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  resource_id = aws_api_gateway_resource.orders.id
  depends_on  = [aws_api_gateway_method.orders_method]

  http_method = each.value.http_method
  status_code = "200"

  response_parameters = {
    "method.response.header.Access-Control-Allow-Origin"  = true
    "method.response.header.Access-Control-Allow-Methods" = true
    "method.response.header.Access-Control-Allow-Headers" = true
  }
}

resource "aws_api_gateway_integration" "orders_integration" {
  for_each = var.api_gateway_lambda_definitions

  rest_api_id             = aws_api_gateway_rest_api.orders_api.id
  resource_id             = aws_api_gateway_resource.orders.id
  depends_on              = [aws_api_gateway_method.orders_method]
  http_method             = aws_api_gateway_method.orders_method[each.key].http_method
  integration_http_method = each.value.http_method
  type                    = "AWS_PROXY"
  uri                     = var.api_gateway_lambda_invoke_arns[each.key]
}

resource "aws_api_gateway_integration_response" "integration_cors_response" {
  for_each = var.api_gateway_lambda_definitions

  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  resource_id = aws_api_gateway_resource.orders.id
  depends_on = [
    aws_api_gateway_method.orders_method,
    aws_api_gateway_integration.orders_integration
  ]

  http_method = each.value.http_method
  status_code = "200"

  response_parameters = {
    "method.response.header.Access-Control-Allow-Origin"  = "'*'"
    "method.response.header.Access-Control-Allow-Headers" = "'Content-Type'"
    "method.response.header.Access-Control-Allow-Methods" = "'OPTIONS,GET,POST,PUT,DELETE'"
  }
}

resource "aws_api_gateway_deployment" "orders_api_deployment" {
  rest_api_id = aws_api_gateway_rest_api.orders_api.id

  triggers = {
    redeploy = sha1(jsonencode(aws_api_gateway_rest_api.orders_api.body))
  }

  lifecycle {
    create_before_destroy = true
  }

  depends_on = [
    aws_api_gateway_method.orders_method,
    aws_api_gateway_integration.orders_integration
  ]
}

resource "aws_api_gateway_stage" "orders_api_stage" {
  rest_api_id   = aws_api_gateway_rest_api.orders_api.id
  deployment_id = aws_api_gateway_deployment.orders_api_deployment.id
  stage_name    = var.env
  depends_on    = [aws_api_gateway_deployment.orders_api_deployment]
}
