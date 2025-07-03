variable "aws_account_id" {
  type = string
}

variable "aws_region" {
  type = string
}

variable "env" {
  type = string
}

variable "batch_job_definitions" {
  type = map(object({
    job_name = string
  }))
}
