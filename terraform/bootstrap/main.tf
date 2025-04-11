provider "aws" {
  region = var.aws_region
}

resource "aws_s3_bucket" "terraform_state" {
  bucket = "terraform-state-${var.env}"
  acl    = "private"
  tags = {
    Name = "Terraform State Bucket"
  }
}

# To prevent accidential overwrites, we can enable versioning on the S3 bucket
# resource "aws_s3_bucket_versioning" "enable_versioning" {
#   bucket = aws_s3_bucket.terraform_state.id

#   versioning_configuration {
#     status = "Enabled"
#   }
# }