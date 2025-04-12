# terraform/main.tf
terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0" 
    }
  }
  backend "s3" {
    bucket  = "centralized-terraform-state-holder"
    key     = "rhyme-net-dev/terraform.tfstate"
    encrypt = true
    region  = "eu-central-1"
    dynamodb_table = "terraform-state-lock-dev"  
  }
}

provider "aws" {
  region = var.aws_region
}

# module "lambdas" {
#   source                      = "./modules/lambdas"
#   aws_account_id              = var.aws_account_id
#   aws_region                  = var.aws_region
#   env                         = var.env
#   ecr_net_lambdas_repo_prefix = var.ecr_net_lambdas_repo_prefix
#   lambda_definitions = {
#     "new-order-lambda" = {
#       lambda_name = "new-order-lambda-${var.env}"
#       memory_size = 128
#       timeout = 60
#       filename = "../Rhyme.Net.Commands.NewOrder/publish/NewOrder.zip"
#       handler = "Rhyme.Net.Commands.NewOrder::Rhyme.Net.Commands.NewOrder.Function::FunctionHandler"
#       runtime = "dotnet8"
#     },
#   }
# }

# module "ecr" {
#   source = "./modules/ecr"
#   environment = var.environment
# }

module "dynamodb" {
  source = "./modules/dynamodb"
  env = var.env
}