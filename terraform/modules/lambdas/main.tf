resource "aws_lambda_function" "lambda" {
  for_each      = var.api_gateway_lambda_definitions

  function_name = each.key
  filename      = each.value.filename # In case deployed as CONTAINER IMAGE: REMOVE - filename and SET - package_type  = "Image" 
  # source_code_hash = each.value.source_code_hash # In case deployed as CONTAINER IMAGE: REMOVE - source_code_hash
  handler       = each.value.handler  # In case deployed as CONTAINER IMAGE: REMOVE - handler and SET - image_uri = "${var.aws_account_id}.dkr.ecr.${var.aws_region}.amazonaws.com/${var.ecr_net_lambdas_repo_prefix}/${each.value.lambda_name}-${var.env}:latest"
  runtime       = each.value.runtime  # In case deployed as CONTAINER IMAGE: REMOVE - runtime
  memory_size   = each.value.memory_size
  timeout       = each.value.timeout
  role          = aws_iam_role.lambda_exec.arn
}

resource "aws_iam_role" "lambda_exec" {
  name = "lambda_exec_role"

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

resource "aws_iam_policy" "lambda_policy" {
  name = "lambda_policy"
  
  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action = [
          "logs:CreateLogGroup",
          "logs:CreateLogStream",
          "logs:PutLogEvents",
        ],
        Effect = "Allow",
        Resource = "arn:aws:logs:*:*:*"
      },
      #Add more policies as needed for lambda execution
    ]
  })
}

resource "aws_iam_role_policy_attachment" "lambda_policy_attach" {
  role       = aws_iam_role.lambda_exec.name
  policy_arn = aws_iam_policy.lambda_policy.arn
}