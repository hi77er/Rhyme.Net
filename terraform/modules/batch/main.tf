resource "aws_ecr_repository" "batch_jobs_repo" {
  name = "batch-jobs-repo-${var.env}"
}

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
  name               = "aws-batch-service-role-${var.env}"
  assume_role_policy = data.aws_iam_policy_document.batch_policy.json
}

resource "aws_iam_role_policy_attachment" "batch_policy_attachment" {
  role       = aws_iam_role.batch_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSBatchServiceRole"
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
  compute_resources {
    max_vcpus          = 16
    subnets            = ["<YOUR_SUBNET_ID_1>", "<YOUR_SUBNET_ID_2>"]
    security_group_ids = [aws_security_group.batch_security_group.id]
    type               = "FARGATE"
  }
  service_role = aws_iam_role.batch_role.arn
  type         = "MANAGED"
}

resource "aws_batch_job_queue" "coupon_generatio_job_queue" {
  name     = "coupon-generatio-job-queue"
  state    = "ENABLED"
  priority = 1

  compute_environment_order {
    order               = 1
    compute_environment = aws_batch_compute_environment.coupon_generation_fargate_env.arn
  }
}

resource "aws_batch_job_definition" "coupon_generation_job_def" {
  for_each = var.batch_job_definitions
  name     = "${each.value.job_name}-def"
  type     = "container"

  container_properties = jsonencode({
    image            = "${aws_ecr_repository.batch_jobs_repo.repository_url}:${each.value.job_name}"
    vcpus            = 1
    memory           = 1024
    command          = []
    jobRoleArn       = aws_iam_role.ecs_task_execution_role.arn
    executionRoleArn = aws_iam_role.ecs_task_execution_role.arn
    networkConfiguration = {
      assignPublicIp = "ENABLED"
    }
    platformCapabilities = ["FARGATE"]
  })
}
