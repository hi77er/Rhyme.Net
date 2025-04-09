
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

variable "ecr_net_lambdas_repo_prefix" {
  description = "Name of the ECR repository used to deploy .Net lambda containers to"
  default     = "dotnet8-lambda-repo"
}
