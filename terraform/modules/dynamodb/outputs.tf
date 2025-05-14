output "dynamodb_stream_arn" {
  description = "ARN of the DynamoDB stream of the events table"
  value       = aws_dynamodb_table.events.stream_arn
}