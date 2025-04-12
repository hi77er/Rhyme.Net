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
        Principal = {
          AWS = "arn:aws:iam::533792392727:user/rhume-net-github-actions-user"
        }
        Action    = [
          "s3:GetObject",
          "s3:PutObject",
          "s3:ListBucket",
          "s3:DeleteObject"
        ]
        Resource  = [
          "${aws_s3_bucket.terraform_state.arn}",       # Bucket-level actions
          "${aws_s3_bucket.terraform_state.arn}/*"     # Object-level actions
        ]
      }
    ]
  })
}
