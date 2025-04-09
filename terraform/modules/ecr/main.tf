# resource "aws_ecr_repository" "lambda_repo" {
#   name                 = "lambda-repo-${var.environment}" # Replace with your repository name
#   image_tag_mutability = "MUTABLE"
# }