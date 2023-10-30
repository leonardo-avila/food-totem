terraform {
  required_providers {
    kubernetes = {
      source = "hashicorp/kubernetes"
    }
  }
}

provider "kubernetes" {
  config_context_cluster = "docker-desktop"
  config_path = "~/.kube/config"
}

resource "kubernetes_service" "food-totem-svc" {
  metadata {
    name = "food-totem-svc"
    labels = {
      app = "food-totem-api"
    }
  }
  spec {
    type = "LoadBalancer"
    selector = {
      app = "food-totem-api"
    }
    port {
      port        = 8080
      target_port = 80
      node_port = 30001
    }
  }
}

resource "kubernetes_deployment" "food-totem-api" {
  metadata {
    name = "food-totem-deployment"
    labels = {
      app = "food-totem-api"
    }
  }
  spec {
    replicas = 2
    selector {
      match_labels = {
        app = "food-totem-api"
      }
    }
    template {
      metadata {
        name = "food-totem-api"
        labels = {
          app = "food-totem-api"
        }
      }
      spec {
        restart_policy = "Always"
        container {
          image = "leonardoavila98/food-totem-api:latest"
          name  = "food-totem-api"
          image_pull_policy = "IfNotPresent"
          port {
            container_port = 80
          }
        }
      }
    }
  }
}