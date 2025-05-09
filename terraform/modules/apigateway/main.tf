resource "aws_api_gateway_rest_api" "orders_api" {
  name        = "OrdersAPI"
  description = "API Gateway for orders service"
}



resource "aws_iam_role" "apigateway_logging_role" {
  name = "APIGatewayLoggingRole"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "apigateway.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_policy" "apigateway_logging_policy" {
  name        = "APIGatewayLoggingPolicy"
  description = "Policy to allow API Gateway logging to CloudWatch"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "logs:CreateLogGroup",
          "logs:CreateLogStream",
          "logs:DescribeLogGroups",
          "logs:DescribeLogStreams",
          "logs:PutLogEvents"
        ]
        Resource = "*"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "attach_logging_policy" {
  role       = aws_iam_role.apigateway_logging_role.name
  policy_arn = aws_iam_policy.apigateway_logging_policy.arn
}

resource "aws_api_gateway_account" "gateway_account_settings" {
  cloudwatch_role_arn = aws_iam_role.apigateway_logging_role.arn
}



resource "aws_api_gateway_rest_api_policy" "orders_api_policy" {
  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  policy = jsonencode({
    Version   = "2012-10-17"
    Statement = [
      {
        Effect    = "Allow"
        Principal = "*"
        Action    = "execute-api:Invoke"
        Resource  = "${aws_api_gateway_rest_api.orders_api.execution_arn}/*/*"
      },
      {
        Effect    = "Allow"
        Principal = {
          "Service": "apigateway.amazonaws.com"
        }
        Action    = "lambda:InvokeFunction"
        Resource  = "arn:aws:lambda:${var.aws_region}:${var.aws_account_id}:function:*"
      }
    ]
  })
}

resource "aws_api_gateway_resource" "orders" {
  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  parent_id   = aws_api_gateway_rest_api.orders_api.root_resource_id
  path_part   = "orders"
}

resource "aws_api_gateway_stage" "orders_api_stage" {
  rest_api_id   = aws_api_gateway_rest_api.orders_api.id
  deployment_id = aws_api_gateway_deployment.orders_api_deployment.id
  stage_name    = var.env


  access_log_settings {
    destination_arn = aws_cloudwatch_log_group.api_gateway_logs.arn
    format          = jsonencode({
      requestId        = "$context.requestId"
      ip               = "$context.identity.sourceIp"
      caller           = "$context.identity.caller"
      user             = "$context.identity.user"
      requestTime      = "$context.requestTime"
      httpMethod       = "$context.httpMethod"
      resourcePath     = "$context.resourcePath"
      status           = "$context.status"
      responseLatency  = "$context.responseLatency"
      errorMessage     = "$context.error.message"
    })
  }

  tags = {
    Environment = var.env
  }
}

resource "aws_cloudwatch_log_group" "api_gateway_logs" {
  name = "/aws/apigateway/orders-api"

  retention_in_days = 1
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

resource "aws_api_gateway_deployment" "orders_api_deployment" {
  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  depends_on  = [aws_api_gateway_integration.orders_integration]
}
