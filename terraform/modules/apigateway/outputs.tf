output "api_id" {
  value = aws_api_gateway_rest_api.orders_api.id
}

output "orders_resource_id" {
  value = aws_api_gateway_resource.orders.id
}