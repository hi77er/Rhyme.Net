variable "aws_account_id" {
  type = string
}

variable "aws_region" {
  type = string
}

variable "env" {
  type = string
}

variable "api_gateway_lambda_definitions" {
  type = map(object({
    lambda_name   = string
    memory_size   = number
    timeout       = number
    # filename      = string # Add filename for the zip archive
    handler       = string # Add handler function
    runtime       = string # Add runtime (e.g., "dotnet8", "python")
    http_method   = string
  }))
}

variable "api_gateway_lambda_invoke_arns" {
  description = "Invoke ARNs for lambda functions"
  type        = map(string)
}