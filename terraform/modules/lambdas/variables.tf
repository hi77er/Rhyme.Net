variable "aws_account_id" {
  type = string
}

variable "aws_region" {
  type = string
}

# variable "ecr_net_lambdas_repo_prefix" {
#   type = string
# }

variable "env" {
  type = string
}

variable "api_gateway_lambda_definitions" {
  type = map(object({
    # filename      = string # Add filename for the zip archive
    lambda_name = string
    memory_size = number
    timeout     = number
    handler     = string # Add handler function
    runtime     = string # Add runtime (e.g., "dotnet8", "python")

    http_method   = string
    resource_path = string
  }))
}

variable "dynamodb_lambda_definitions" {
  type = map(object({
    # filename      = string # Add filename for the zip archive
    lambda_name = string
    memory_size = number
    timeout     = number
    handler     = string # Add handler function
    runtime     = string # Add runtime (e.g., "dotnet8", "python")

    dynamodb_stream_arn = string
  }))
}
