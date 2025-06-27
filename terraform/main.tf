# terraform/main.tf
terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
  backend "s3" {
    bucket         = "centralized-terraform-state-holder"
    key            = "rhyme-net-dev/terraform.tfstate"
    encrypt        = true
    region         = "eu-central-1"
    dynamodb_table = "terraform-state-lock-dev"
  }
}

provider "aws" {
  region = var.aws_region
}

module "dynamodb" {
  source = "./modules/dynamodb"
  env    = var.env
}

module "lambdas" {
  source                         = "./modules/lambdas"
  aws_account_id                 = var.aws_account_id
  aws_region                     = var.aws_region
  env                            = var.env
  api_gateway_lambda_definitions = var.api_gateway_lambda_definitions
  generic_api_arn                = module.apigateway.generic_api_arn
  dynamodb_lambda_definitions    = var.dynamodb_lambda_definitions
  dynamodb_tables                = module.dynamodb.dynamodb_tables
  # ecr_net_lambdas_repo_prefix     = var.ecr_net_lambdas_repo_prefix
}

module "apigateway" {
  source                         = "./modules/apigateway"
  aws_account_id                 = var.aws_account_id
  aws_region                     = var.aws_region
  env                            = var.env
  api_gateway_lambda_definitions = var.api_gateway_lambda_definitions
  api_gateway_lambda_invoke_arns = module.lambdas.api_gateway_lambda_invoke_arns
}