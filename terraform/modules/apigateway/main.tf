resource "aws_api_gateway_rest_api" "orders_api" {
  name        = "OrdersAPI"
  description = "API Gateway for orders service"
}

resource "aws_api_gateway_resource" "orders" {
  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  parent_id   = aws_api_gateway_rest_api.orders_api.root_resource_id
  path_part   = "orders"
}

resource "aws_api_gateway_method" "orders_method" {
  for_each      = var.api_gateway_lambda_definitions
  
  rest_api_id   = aws_api_gateway_rest_api.orders_api.id
  resource_id   = aws_api_gateway_resource.orders.id
  http_method   = each.value.http_method
  authorization = "NONE"
}

resource "aws_api_gateway_integration" "orders_integration" {
  for_each    = var.api_gateway_lambda_definitions

  rest_api_id = aws_api_gateway_rest_api.orders_api.id
  resource_id = aws_api_gateway_resource.orders.id
  http_method = aws_api_gateway_method.orders_method[each.key].http_method
  integration_http_method = each.value.http_method
  type        = "AWS_PROXY"
  uri         = var.api_gateway_lambda_invoke_arns[each.key]
}