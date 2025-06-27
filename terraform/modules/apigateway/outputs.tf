output "orders_method_ids" {
  value = { for k, v in aws_api_gateway_method.generic_method : k => v.id }
}

output "generic_api_arn" {
  value = aws_api_gateway_rest_api.generic_api.execution_arn
}
