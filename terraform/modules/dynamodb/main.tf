variable "env" {
  description = "Environment name"
  type        = string
}

resource "aws_dynamodb_table" "events" {
  name             = "events-${var.env}"
  billing_mode     = "PAY_PER_REQUEST"
  hash_key         = "aggregateName"
  range_key        = "aggregateId"
  stream_enabled   = true
  stream_view_type = "NEW_AND_OLD_IMAGES" # Choose from KEYS_ONLY, NEW_IMAGE, OLD_IMAGE, NEW_AND_OLD_IMAGES

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
  name         = "orders-${var.env}"
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "id"
  range_key    = "storeId"

  attribute {
    name = "id"
    type = "S"
  }

  attribute {
    name = "storeId"
    type = "S"
  }
}

resource "aws_dynamodb_table" "coupons" {
  name         = "coupons-${var.env}"
  billing_mode = "PAY_PER_REQUEST"
  hash_key  = "id"
  range_key = "campaignId"

  attribute {
    name = "id"
    type = "S"
  }

  attribute {
    name = "campaignId"
    type = "S"
  }
}
