variable "env" {
  description = "Environment name"
  type        = string
}

resource "aws_dynamodb_table" "events" {
  name           = "events-${var.env}"
  billing_mode   = "PAY_PER_REQUEST"
  hash_key       = "aggregateName"
  range_key      = "aggregateId"

  attribute {
    name = "aggregateName"
    type = "S"
  }
  
  attribute {
    name = "aggregateId"
    type = "S"
  }
}

resource "aws_dynamodb_table" "orders" {
  name           = "orders-${var.env}"
  billing_mode   = "PAY_PER_REQUEST"
  hash_key       = "id"
  range_key      = "storeId"

  attribute {
    name = "id"
    type = "S"
  }
  
  attribute {
    name = "storeId"
    type = "S"
  }
}
