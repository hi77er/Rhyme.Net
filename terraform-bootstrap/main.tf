# terraform/main.tf
terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0" 
    }
  }
}

provider "aws" {
  region = var.aws_region
}

resource "aws_s3_bucket" "terraform_state" {
  bucket = "rhyme-net-terraform-state-${var.env}"

  tags = {
    Name = "Terraform State Bucket"
  }
}

# To prevent accidential overwrites, we can enable versioning on the S3 bucket
resource "aws_s3_bucket_versioning" "enable_versioning" {
  bucket = aws_s3_bucket.terraform_state.id

  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "example_encryption" {
  bucket = aws_s3_bucket.terraform_state.id

  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256" # You can replace with "aws:kms" for KMS encryption
    }
  }
}

resource "aws_s3_bucket_policy" "example_bucket_policy" {
  bucket = aws_s3_bucket.terraform_state.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect   = "Allow"
        Principal = "*"
        Action   = [
          "s3:GetObject",
          "s3:PutObject",
          "s3:DeleteObject"
        ]
        Resource = [
          "${aws_s3_bucket.terraform_state.arn}/*"     # Object-level actions
        ]
        Resource = "$"
      },
      {
        Effect   = "Allow"
        Principal = "*"
        Action   = [
          "s3:ListObjectsV2"
        ]
        Resource = [
          "${aws_s3_bucket.terraform_state.arn}",       # Bucket-level actions
        ]
        Resource = "$"
      }
    ]
  })
}

resource "aws_dynamodb_table" "terraform_state_lock" {
  name           = "terraform-state-lock-${var.env}"
  billing_mode   = "PAY_PER_REQUEST" # On-demand pricing; change to PROVISIONED if needed
  hash_key       = "LockId"          # Partition key column

  attribute {
    name = "LockId"
    type = "S" # "S" stands for String type
  }

  tags = {
    Name        = "TerraformStateLockDev"
    Environment = "Development"
  }
}