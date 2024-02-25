variable "lab_account_id" {
  description = "AWS Labs account ID"
}

variable "lab_account_region" {
  description = "AWS Labs account region"
  default = "us-west-2"
}

variable "mysql_user" {
  description = "MySQL user"
}

variable "mysql_password" {
  description = "MySQL password"
}

variable "food_totem_api_image" {
  description = "Food Totem API image"
}

variable "rabbitMQ_user" {
  type = string
  sensitive = true
}

variable "rabbitMQ_password" {
  type = string
  sensitive = true
}

variable "mailtrap_host" {
  type = string
  sensitive = true
}

variable "mailtrap_user" {
  type = string
  sensitive = true
}

variable "mailtrap_password" {
  type = string
  sensitive = true
}