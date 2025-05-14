output "dynamodb_tables" {
  value = [
    {
      key   = "events-dev"
      value = aws_dynamodb_table.events.stream_arn
    }
  ]
}
