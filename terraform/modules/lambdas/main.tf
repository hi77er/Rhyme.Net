data "archive_file" "dummy_lambda_function" {
  type        = "zip"
  output_path = "dummy_lambda_function.zip"

  source {
    content  = "Placeholder file"
    filename = "dummy.txt"
  }
}

###
# API Gateway Lambda Functions
### 

resource "aws_lambda_function" "apigateway_lambdas" {
  # source_code_hash = each.value.source_code_hash # In case deployed as CONTAINER IMAGE: REMOVE - source_code_hash
  for_each      = var.api_gateway_lambda_definitions
  function_name = each.key
  filename      = data.archive_file.dummy_lambda_function.output_path # In case deployed as CONTAINER IMAGE: REMOVE - filename and SET - package_type  = "Image" 
  handler       = each.value.handler                                  # In case deployed as CONTAINER IMAGE: REMOVE - handler and SET - image_uri = "${var.aws_account_id}.dkr.ecr.${var.aws_region}.amazonaws.com/${var.ecr_net_lambdas_repo_prefix}/${each.value.lambda_name}-${var.env}:latest"
  runtime       = each.value.runtime                                  # In case deployed as CONTAINER IMAGE: REMOVE - runtime
  memory_size   = each.value.memory_size
  timeout       = each.value.timeout
  role          = aws_iam_role.apigateway_lambda_role.arn
}

resource "aws_iam_role" "apigateway_lambda_role" {
  name = "apigateway_lambda_role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action = "sts:AssumeRole",
        Effect = "Allow",
        Principal = {
          Service = "lambda.amazonaws.com"
        }
      },
    ]
  })
}

resource "aws_iam_policy" "apigateway_lambda_policy" {
  name = "apigateway_lambda_policy"

  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action = [
          "logs:CreateLogGroup",
          "logs:CreateLogStream",
          "logs:PutLogEvents",
        ],
        Effect   = "Allow",
        Resource = "arn:aws:logs:*:*:*"
      },
      #Add more policies as needed for lambda execution
    ]
  })
}

resource "aws_iam_role_policy_attachment" "apigateway_lambda_policy_attach" {
  role       = aws_iam_role.apigateway_lambda_role.name
  policy_arn = aws_iam_policy.apigateway_lambda_policy.arn
}

###
# DynamoDB Lambda Functions
###

resource "aws_lambda_function" "dynamodb_lambdas" {
  for_each      = var.dynamodb_lambda_definitions
  function_name = each.key
  filename      = data.archive_file.dummy_lambda_function.output_path # In case deployed as CONTAINER IMAGE: REMOVE - filename and SET - package_type  = "Image" 
  handler       = each.value.handler
  runtime       = each.value.runtime
  role          = aws_iam_role.dynamodb_lambda_role.arn
  memory_size   = each.value.memory_size
  timeout       = each.value.timeout
}

resource "aws_lambda_event_source_mapping" "dynamodb_stream_mapping" {
  for_each          = var.dynamodb_lambda_definitions
  event_source_arn  = lookup({ for item in var.dynamodb_tables : item.key => item.value }, each.value.table_name, "")
  function_name     = each.key
  starting_position = "LATEST"

  depends_on = [aws_lambda_function.dynamodb_lambdas]
}

resource "aws_iam_role" "dynamodb_lambda_role" {
  name = "lambda_execution_role"

  assume_role_policy = jsonencode({
    Version : "2012-10-17",
    Statement : [
      {
        Action : "sts:AssumeRole",
        Effect : "Allow",
        Principal : {
          "Service" : "lambda.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_policy" "dynamodb_lambda_policy" {
  name        = "lambda_dynamodb_access_policy"
  description = "Policy to allow Lambda to read from DynamoDB stream"

  policy = jsonencode({
    Version : "2012-10-17",
    Statement : [
      {
        Action : [
          "dynamodb:GetRecords",
          "dynamodb:DescribeStream",
          "dynamodb:ListStreams",
          "dynamodb:GetShardIterator"
        ],
        Effect : "Allow",
        Resource : "*"
      },
      {
        Action : [
          "logs:CreateLogGroup",
          "logs:CreateLogStream",
          "logs:PutLogEvents"
        ],
        Effect : "Allow",
        Resource : "arn:aws:logs:*:*:*"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "lambda_policy_attachment" {
  policy_arn = aws_iam_policy.dynamodb_lambda_policy.arn
  role       = aws_iam_role.dynamodb_lambda_role.name
}

###
# SQS Lambda Functions
###
