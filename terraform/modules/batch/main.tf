data "aws_caller_identity" "current" {}


data "aws_iam_policy_document" "batch_policy" {
  statement {
    actions = ["sts:AssumeRole"]
    principals {
      type        = "Service"
      identifiers = ["batch.amazonaws.com"]
    }
  }
}

resource "aws_iam_role" "batch_role" {
  depends_on         = [data.aws_iam_policy_document.batch_policy]
  name               = "aws-batch-service-role-${var.env}"
  assume_role_policy = data.aws_iam_policy_document.batch_policy.json
}

resource "aws_iam_role_policy_attachment" "batch_full_access_attachment" {
  depends_on = [aws_iam_role.batch_role]
  role       = aws_iam_role.batch_role.name
  policy_arn = "arn:aws:iam::aws:policy/AWSBatchFullAccess"
  lifecycle {
    prevent_destroy = true
  }
}

resource "aws_iam_role_policy" "ecs_cluster_management_policy" {
  depends_on = [aws_iam_role.batch_role]
  name       = "ecs-cluster-management-policy-${var.env}"
  role       = aws_iam_role.batch_role.id

  lifecycle {
    prevent_destroy = true
  }

  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect = "Allow",
        Action = [
          "ecs:CreateCluster",
          "ecs:DeleteCluster",
          "ecs:ListClusters",
          "ecs:DescribeClusters",
          "ecs:TagResource",
          "ecs:UntagResource"
        ],
        Resource = "*"
      }
    ]
  })
}

resource "aws_ecr_repository" "batch_jobs_repo" {
  name = "batch-jobs-repo-${var.env}"
}

data "aws_iam_policy_document" "ecs_task_policy" {
  statement {
    actions = ["sts:AssumeRole"]
    principals {
      type        = "Service"
      identifiers = ["ecs-tasks.amazonaws.com"]
    }
  }
}

resource "aws_iam_role" "ecs_task_execution_role" {
  name               = "ecsTaskExecutionRole-${var.env}"
  assume_role_policy = data.aws_iam_policy_document.ecs_task_policy.json
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_policy_attachment" {
  depends_on = [aws_batch_compute_environment.coupon_generation_fargate_env]
  role       = aws_iam_role.ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_security_group" "batch_security_group" {
  name        = "batch-fargate-sg-${var.env}"
  description = "Allow all egress"
  vpc_id      = "vpc-0c368a4f2b4097c4d"

  ingress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_batch_compute_environment" "coupon_generation_fargate_env" {
  compute_environment_name = "coupon-generation-fargate-ce-${var.env}"
  depends_on = [
    aws_iam_role.batch_role,
    aws_security_group.batch_security_group
  ]
  compute_resources {
    max_vcpus          = 16
    subnets            = ["subnet-0a420baeba3391c2f", "subnet-0d3dfd7e7e2b58ed4"]
    security_group_ids = [aws_security_group.batch_security_group.id]
    type               = "FARGATE"
  }
  service_role = aws_iam_role.batch_role.arn
  type         = "MANAGED"
}

resource "aws_batch_job_queue" "coupon_generation_job_queue" {
  depends_on = [aws_batch_compute_environment.coupon_generation_fargate_env]
  name       = "coupon-generation-job-queue"
  state      = "ENABLED"
  priority   = 1

  compute_environment_order {
    order               = 1
    compute_environment = aws_batch_compute_environment.coupon_generation_fargate_env.arn
  }
}

resource "aws_batch_job_definition" "coupon_generation_job_def" {
  depends_on = [aws_batch_compute_environment.coupon_generation_fargate_env]
  for_each   = var.batch_job_definitions
  name       = "${each.value.job_name}-def"
  type       = "container"

  container_properties = jsonencode({
    image                = "${aws_ecr_repository.batch_jobs_repo.repository_url}:${each.value.job_name}"
    vcpus                = 1
    memory               = 1024
    command              = []
    jobRoleArn           = aws_iam_role.ecs_task_execution_role.arn
    executionRoleArn     = aws_iam_role.ecs_task_execution_role.arn
    platformCapabilities = ["FARGATE"]
  })
}
