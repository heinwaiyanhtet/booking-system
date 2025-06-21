variable "aws_region" {
  description = "AWS region"
  default     = "us-east-1"
}

variable "cluster_name" {
  description = "Name for ECS cluster and related resources"
  default     = "booking-api"
}

variable "image" {
  description = "ECR image URI"
  type        = string
}

variable "container_name" {
  default = "booking-api"
}

variable "container_port" {
  default = 80
}

variable "desired_count" {
  default = 1
}

variable "environment" {
  description = "Environment variables for the container"
  type        = list(object({ name = string, value = string }))
  default     = []
}