
variable "aws_account_id" {
  description = "ID of the AWS Account to deploy to"
  default     = "XXXXXXX"
}

variable "aws_region" {
  description = "AWS Region to deploy to"
  default     = "eu-central-1"
}

variable "env" {
  description = "Short name for the deployment environment (e.g., dev, prod)"
  default     = "dev"
}

variable "environment" {
  description = "Full name for the deployment environment (e.g., development, production)"
  default     = "development"
}

# variable "ecr_net_lambdas_repo_prefix" {
#   description = "Name of the ECR repository used to deploy .Net lambda containers to"
#   default     = "dotnet8-lambda-repo"
# }

variable "api_gateway_lambda_definitions" {
  default = {
    "NewOrderLambda-dev" = {
      lambda_name = "NewOrderLambda-dev"
      # filename = "../Rhyme.Net.Commands.NewOrderLambda/src/Rhyme.Net.Commands.NewOrderLambda/publish/NewOrderLambda.zip"
      memory_size = 128
      timeout     = 60
      handler     = "Rhyme.Net.Commands.NewOrderLambda::Rhyme.Net.Commands.NewOrderLambda.Function::FunctionHandler"
      runtime     = "dotnet8"
      http_method   = "POST"
    }
    "GetOrdersLambda-dev" = {
      lambda_name = "GetOrdersLambda-dev"
      memory_size = 128
      timeout     = 60
      # filename    = "../Rhyme.Net.Queries.GetOrdersLambda/src/Rhyme.Net.Queries.GetOrdersLambda/publish/GetOrdersLambda.zip"
      handler     = "Rhyme.Net.Queries.GetOrdersLambda::Rhyme.Net.Queries.GetOrdersLambda.Function::FunctionHandler"
      runtime     = "dotnet8"
      http_method   = "GET"
    }
  }
}
