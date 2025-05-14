output "api_gateway_lambda_invoke_arns" {
  value = { for k, v in aws_lambda_function.apigateway_lambdas : k => v.invoke_arn }
}
