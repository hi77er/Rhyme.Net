resource "aws_dynamodb_table" "terraform_state_lock" {
  name           = "terraform-state-lock-${var.env}"
  billing_mode   = "PAY_PER_REQUEST" # On-demand pricing; change to PROVISIONED if needed
  hash_key       = "LockID"          # Partition key column

  attribute {
    name = "LockID"
    type = "S" # "S" stands for String type
  }

  tags = {
    Name        = "TerraformStateLockDev"
    Environment = "Development"
  }
}