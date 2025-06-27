resource "aws_api_gateway_rest_api" "generic_api" {
  name        = "OrdersAPI"
  description = "API Gateway for orders service"
}

# resource "aws_cloudwatch_log_group" "api_gw_logs" {
#   name              = "/aws/api-gateway/orders-api"
#   retention_in_days = 1
# }

# resource "aws_iam_role" "apigateway_logging_role" {
#   name = "APIGatewayLoggingRole"

#   assume_role_policy = jsonencode({
#     Version = "2012-10-17"
#     Statement = [
#       {
#         Effect = "Allow"
#         Principal = {
#           Service = ["apigateway.amazonaws.com", "lambda.amazonaws.com"]
#         }
#         Action = "sts:AssumeRole"
#       }
#     ]
#   })
# }

# resource "aws_iam_policy" "apigateway_logging_policy" {
#   name        = "APIGatewayLoggingPolicy"
#   description = "Policy to allow API Gateway logging to CloudWatch"
#   depends_on  = [aws_iam_role.apigateway_logging_role]

#   policy = jsonencode({
#     Version = "2012-10-17"
#     Statement = [
#       {
#         Effect = "Allow"
#         Action = [
#           "logs:*",
#           "xray:*",
#         ]
#         Resource = "*"
#       }
#     ]
#   })
# }

# resource "aws_iam_policy_attachment" "api_gw_logs_attachment" {
#   name       = "APIGatewayLogsPolicyAttachment"
#   policy_arn = aws_iam_policy.apigateway_logging_policy.arn
#   roles      = [aws_iam_role.apigateway_logging_role.name]
# }

# resource "aws_api_gateway_account" "gateway_account_settings" {
#   cloudwatch_role_arn = aws_iam_role.apigateway_logging_role.arn
# }

resource "aws_api_gateway_rest_api_policy" "generic_api_policy" {
  rest_api_id = aws_api_gateway_rest_api.generic_api.id
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect    = "Allow"
        Principal = "*"
        Action    = "execute-api:Invoke"
        Resource  = "${aws_api_gateway_rest_api.generic_api.execution_arn}/*/*"
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
  source_arn    = "${aws_api_gateway_rest_api.generic_api.execution_arn}/*/*"
}

resource "aws_api_gateway_resource" "api" {
  rest_api_id = aws_api_gateway_rest_api.generic_api.id
  parent_id   = aws_api_gateway_rest_api.generic_api.root_resource_id
  path_part   = "api"
}

resource "aws_api_gateway_resource" "orders" {
  rest_api_id = aws_api_gateway_rest_api.generic_api.id
  parent_id   = aws_api_gateway_resource.api.id
  path_part   = "orders"
}

resource "aws_api_gateway_resource" "orders_by_id" {
  rest_api_id = aws_api_gateway_rest_api.generic_api.id
  parent_id   = aws_api_gateway_resource.orders.id
  path_part   = "{id}"
}

resource "aws_api_gateway_resource" "coupons" {
  rest_api_id = aws_api_gateway_rest_api.generic_api.id
  parent_id   = aws_api_gateway_resource.api.id
  path_part   = "coupons"
}

locals {
  resource_map = {
    "api"      = aws_api_gateway_resource.api.id
    "orders"      = aws_api_gateway_resource.orders.id
    "orders/{id}" = aws_api_gateway_resource.orders_by_id.id
    "coupons"     = aws_api_gateway_resource.coupons.id
  }
}

# resource "aws_api_gateway_method" "options_method" {
#   rest_api_id   = aws_api_gateway_rest_api.generic_api.id
#   resource_id   = aws_api_gateway_resource.orders.id
#   http_method   = "OPTIONS"
#   authorization = "NONE"
# }

# resource "aws_api_gateway_method_response" "options_response" {
#   rest_api_id = aws_api_gateway_rest_api.generic_api.id
#   resource_id = aws_api_gateway_resource.orders.id
#   http_method = "OPTIONS"
#   status_code = "200"
#   depends_on  = [aws_api_gateway_method.options_method]

#   response_parameters = {
#     "method.response.header.Access-Control-Allow-Origin"  = true
#     "method.response.header.Access-Control-Allow-Methods" = true
#     "method.response.header.Access-Control-Allow-Headers" = true
#   }
# }

# resource "aws_api_gateway_integration" "options_integration" {
#   rest_api_id             = aws_api_gateway_rest_api.generic_api.id
#   resource_id             = aws_api_gateway_resource.orders.id
#   http_method             = "OPTIONS"
#   integration_http_method = "OPTIONS"
#   type                    = "MOCK"
#   depends_on              = [aws_api_gateway_method.options_method]

#   request_templates = {
#     "application/json" = jsonencode(
#       {
#         statusCode = 200
#       }
#     )
#   }
# }

# resource "aws_api_gateway_integration_response" "options_integration_response" {
#   rest_api_id = aws_api_gateway_rest_api.generic_api.id
#   resource_id = aws_api_gateway_resource.orders.id
#   depends_on  = [aws_api_gateway_integration.options_integration]
#   http_method = "OPTIONS"
#   status_code = "200"

#   response_parameters = {
#     "method.response.header.Access-Control-Allow-Origin"  = "'*'"
#     "method.response.header.Access-Control-Allow-Methods" = "'OPTIONS,GET,POST,PUT,DELETE'"
#     "method.response.header.Access-Control-Allow-Headers" = "'Content-Type'"
#   }
# }

resource "aws_api_gateway_method" "generic_method" {
  # resource_id = aws_api_gateway_resource.orders.id
  for_each      = var.api_gateway_lambda_definitions
  resource_id   = lookup(local.resource_map, each.value.resource_path, "INVALID_RESOURCE")
  rest_api_id   = aws_api_gateway_rest_api.generic_api.id
  http_method   = each.value.http_method
  authorization = "NONE"
}

resource "aws_api_gateway_method_response" "generic_method_cors_response" {
  for_each = var.api_gateway_lambda_definitions

  # resource_id = aws_api_gateway_resource.orders.id
  rest_api_id = aws_api_gateway_rest_api.generic_api.id
  resource_id = lookup(local.resource_map, each.value.resource_path, "INVALID_RESOURCE")
  depends_on  = [aws_api_gateway_method.generic_method]

  http_method = each.value.http_method
  status_code = "200"

  response_parameters = {
    "method.response.header.Access-Control-Allow-Origin"  = true
    "method.response.header.Access-Control-Allow-Methods" = true
    "method.response.header.Access-Control-Allow-Headers" = true
  }
}

resource "aws_api_gateway_integration" "generic_resource_integration" {
  # resource_id             = aws_api_gateway_resource.orders.id
  for_each                = var.api_gateway_lambda_definitions
  rest_api_id             = aws_api_gateway_rest_api.generic_api.id
  resource_id             = lookup(local.resource_map, each.value.resource_path, "INVALID_RESOURCE")
  depends_on              = [aws_api_gateway_method.generic_method]
  http_method             = aws_api_gateway_method.generic_method[each.key].http_method
  integration_http_method = each.value.http_method
  type                    = "AWS_PROXY"
  uri                     = var.api_gateway_lambda_invoke_arns[each.key]
}

resource "aws_api_gateway_integration_response" "integration_cors_response" {
  # resource_id = aws_api_gateway_resource.orders.id
  for_each    = var.api_gateway_lambda_definitions
  rest_api_id = aws_api_gateway_rest_api.generic_api.id
  resource_id = lookup(local.resource_map, each.value.resource_path, "INVALID_RESOURCE")
  depends_on = [
    aws_api_gateway_method.generic_method,
    aws_api_gateway_integration.generic_resource_integration
  ]

  http_method = each.value.http_method
  status_code = "200"

  response_parameters = {
    "method.response.header.Access-Control-Allow-Origin"  = "'*'"
    "method.response.header.Access-Control-Allow-Methods" = "'OPTIONS,GET,POST,PUT,DELETE'"
    "method.response.header.Access-Control-Allow-Headers" = "'Content-Type'"
  }
}

resource "aws_api_gateway_deployment" "generic_api_deployment" {
  rest_api_id = aws_api_gateway_rest_api.generic_api.id

  triggers = {
    redeploy = sha1(jsonencode(aws_api_gateway_rest_api.generic_api.body))
  }

  lifecycle {
    create_before_destroy = true
  }

  depends_on = [
    aws_api_gateway_method.generic_method,
    aws_api_gateway_integration.generic_resource_integration
  ]
}

resource "aws_api_gateway_stage" "generic_api_stage" {
  rest_api_id   = aws_api_gateway_rest_api.generic_api.id
  deployment_id = aws_api_gateway_deployment.generic_api_deployment.id
  stage_name    = var.env
  depends_on    = [aws_api_gateway_deployment.generic_api_deployment]

  # access_log_settings {
  #   destination_arn = aws_cloudwatch_log_group.api_gw_logs.arn
  #   format = jsonencode({
  #     requestId               = "$context.requestId",
  #     ip                      = "$context.identity.sourceIp",
  #     requestTime             = "$context.requestTime",
  #     httpMethod              = "$context.httpMethod",
  #     routeKey                = "$context.routeKey",
  #     status                  = "$context.status",
  #     responseLatency         = "$context.responseLatency",
  #     protocol                = "$context.protocol",
  #     responseLength          = "$context.responseLength",
  #     requestBody             = "$context.requestBody",
  #     userAgent               = "$context.identity.userAgent",
  #     caller                  = "$context.identity.caller",
  #     user                    = "$context.identity.user",
  #     cognitoIdentity         = "$context.identity.cognitoIdentityId",
  #     apiId                   = "$context.apiId",
  #     resourcePath            = "$context.resourcePath",
  #     authorizer              = "$context.authorizer.claims",
  #     integrationStatus       = "$context.integration.status",
  #     integrationLatency      = "$context.integration.latency",
  #     integrationErrorMessage = "$context.integration.error",
  #     errorMessage            = "$context.error.message",
  #     errorType               = "$context.error.type"
  #   })
  # }
}
