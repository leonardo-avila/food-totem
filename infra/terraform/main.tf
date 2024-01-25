terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
    }
  }
  required_version = ">= 1.3.7"
  backend "s3" {
    bucket                  = "terraform-buckets-food-totem"
    key                     = "food-totem-api/terraform.tfstate"
    region                  = "us-west-2"
  }
}
provider "aws" {
    region = var.lab_account_region
}

data "aws_security_group" "default" {
  name = "default"
}

data "aws_vpc" "default" {
  default = true
}

data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

data "aws_lb" "catalog-api-lb" {
  name = "catalog-api"
}

data "aws_lb" "payment-api-lb" {
  name = "payment-api"
}

data "aws_lb" "demand-api-lb" {
  name = "demand-api"
}

data "aws_lambda_function" "food-totem-jwt" {
  function_name = "food-totem-jwt"
}

locals {
  endpoint_parts = split(":", aws_db_instance.food-totem-mysql.endpoint)
  port = local.endpoint_parts[1]
  catalog_url = "http://${data.aws_lb.catalog-api-lb.dns_name}"
  payment_url = "http://${data.aws_lb.payment-api-lb.dns_name}"
  demand_url = "http://${data.aws_lb.demand-api-lb.dns_name}"
}

resource "aws_db_instance" "food-totem-mysql" {
  engine               = "mysql"
  identifier           = "food-totem-api-db"
  allocated_storage    =  20
  engine_version       = "8.0.33"
  instance_class       = "db.t2.micro"
  username             = var.mysql_user
  password             = var.mysql_password
  parameter_group_name = "food-totem-mysql-parameter"
  vpc_security_group_ids = [data.aws_security_group.default.id]
  skip_final_snapshot  = true
  publicly_accessible =  true
}

resource "aws_ecs_task_definition" "food-totem-api-task" {
  depends_on = [ aws_db_instance.food-totem-mysql ]
  family                   = "food-totem-api"
  network_mode             = "awsvpc"
  execution_role_arn       = "arn:aws:iam::${var.lab_account_id}:role/LabRole"
  cpu                      = 256
  memory                   = 512
  requires_compatibilities = ["FARGATE"]
  container_definitions    = jsonencode([
    {
        "name": "food-totem-api",
        "image": var.food_totem_api_image,
        "essential": true,
        "portMappings": [
            {
              "containerPort": 80,
              "hostPort": 80,
              "protocol": "tcp"
            }
        ],
        "environment": [
            {
                "name": "ConnectionStrings__DefaultConnection",
                "value": join("", ["Server=", element(split(":", aws_db_instance.food-totem-mysql.endpoint), 0), ";Port=", local.port, ";Database=foodtotem;Uid=", var.mysql_user, ";Pwd=", var.mysql_password, ";"])
            },
            {
                "name": "AuthenticationUrl",
                "value": aws_api_gateway_deployment.food-totem-jwt-api-deployment.invoke_url
            },
            {
                "name": "DemandServiceUrl",
                "value": local.demand_url
            },
            {
                "name": "PaymentServiceUrl",
                "value": local.payment_url
            },
            {
                "name": "CatalogServiceUrl",
                "value": local.catalog_url
            }
        ],
        "cpu": 256,
        "memory": 512,
        "logConfiguration": {
            "logDriver": "awslogs",
            "options": {
                "awslogs-group": "food-totem-api-logs",
                "awslogs-region": "us-west-2",
                "awslogs-stream-prefix": "food-totem-api"
            }
        }
    }
  ])
}

resource "aws_lb_target_group" "food-totem-api-tg" {
  name     = "food-totem-api"
  port     = 80
  protocol = "HTTP"
  vpc_id   = data.aws_vpc.default.id
  target_type = "ip"

  health_check {
    enabled = true
    interval = 300
    path = "/health-check"
    protocol = "HTTP"
    timeout = 60
    healthy_threshold = 3
    unhealthy_threshold = 3
  }
}

resource "aws_lb" "food-totem-api-lb" {
  name               = "food-totem-api"
  internal           = false
  load_balancer_type = "application"
  subnets            = data.aws_subnets.default.ids
}

resource "aws_lb_listener" "food-totem-api-lbl" {
  load_balancer_arn = aws_lb.food-totem-api-lb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.food-totem-api-tg.arn
  }
}

resource "aws_ecs_service" "food-totem-api-service" {
  name            = "food-totem-api-service"
  cluster         = "food-totem-ecs"
  task_definition = aws_ecs_task_definition.food-totem-api-task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    security_groups  = [data.aws_security_group.default.id]
    subnets = data.aws_subnets.default.ids
    assign_public_ip = true
  }

  load_balancer {
    container_name = "food-totem-api"
    container_port = 80
    target_group_arn = aws_lb_target_group.food-totem-api-tg.arn
  }

  health_check_grace_period_seconds = 120
}

resource "aws_api_gateway_rest_api" "food-totem-jwt-api" {
  name        = "API Gateway for JWT Generator Lambda"
  description = "Provides a gateway to call the JWT Generator Lambda"
}

resource "aws_api_gateway_method" "food-totem-jwt-proxy" {
  rest_api_id   = aws_api_gateway_rest_api.food-totem-jwt-api.id
  resource_id   = aws_api_gateway_rest_api.food-totem-jwt-api.root_resource_id
  http_method   = "ANY"
  authorization = "NONE"
}

resource "aws_api_gateway_integration" "food-totem-jwt-integration" {
  rest_api_id = aws_api_gateway_rest_api.food-totem-jwt-api.id
  resource_id = aws_api_gateway_method.food-totem-jwt-proxy.resource_id
  http_method = aws_api_gateway_method.food-totem-jwt-proxy.http_method

  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = data.aws_lambda_function.food-totem-jwt.invoke_arn
}

resource "aws_api_gateway_deployment" "food-totem-jwt-api-deployment" {
  depends_on = [
    aws_api_gateway_integration.food-totem-jwt-integration
  ]

  rest_api_id = aws_api_gateway_rest_api.food-totem-jwt-api.id
  stage_name  = "prod"
}

resource "aws_lambda_permission" "food-totem-api-permission" {
  statement_id  = "AllowAPIGatewayInvoke"
  action        = "lambda:InvokeFunction"
  function_name = data.aws_lambda_function.food-totem-jwt.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_api_gateway_rest_api.food-totem-jwt-api.execution_arn}/*/*"
}

resource "aws_cloudwatch_log_group" "food-totem-api-logs" {
  name = "food-totem-api-logs"
  retention_in_days = 1
}