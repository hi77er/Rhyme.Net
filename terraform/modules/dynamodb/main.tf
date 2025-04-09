# resource "aws_dynamodb_table" "stores" {
#   name           = "stores"
#   billing_mode   = "PAY_PER_REQUEST"
#   hash_key       = "id"

#   attribute {
#     name = "id"
#     type = "S"
#   }
# }

# resource "aws_dynamodb_table" "products" {
#   name           = "products"
#   billing_mode   = "PAY_PER_REQUEST"
#   hash_key       = "id"

#   attribute {
#     name = "id"
#     type = "S"
#   }
# }

# resource "aws_dynamodb_table" "ranges" {
#   name           = "ranges"
#   billing_mode   = "PAY_PER_REQUEST"
#   hash_key       = "productId"
#   range_key      = "storeId"

#   attribute {
#     name = "productId"
#     type = "S"
#   }
#   attribute {
#     name = "storeId"
#     type = "S"
#   }
# }

resource "aws_dynamodb_table" "orders" {
  name           = "orders"
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