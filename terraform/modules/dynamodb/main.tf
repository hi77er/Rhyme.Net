variable "env" {
  description = "Environment name"
  type        = string
}

resource "aws_dynamodb_table" "orders" {
  name           = "orders-${var.env}"
  billing_mode   = "PAY_PER_REQUEST"
  hash_key       = "id"

  attribute {
    name = "id"
    type = "S"
  }
  
  attribute {
    name = "storeId"
    type = "S"
  }
  
  global_secondary_index {
    name            = "storeId-index"
    hash_key        = "storeId"
    projection_type = "ALL"
  }
}