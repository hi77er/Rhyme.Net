output "orders_method_ids" {
  value = { for k, v in aws_api_gateway_method.generic_method : k => v.id }
}