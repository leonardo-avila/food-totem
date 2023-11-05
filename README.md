# Food Totem
[![Build](https://github.com/leonardo-avila/food-totem/actions/workflows/build.yml/badge.svg)](https://github.com/leonardo-avila/food-totem/actions/workflows/build.yml)
[![Deploy](https://github.com/leonardo-avila/food-totem/actions/workflows/deploy.yml/badge.svg)](https://github.com/leonardo-avila/food-totem/actions/workflows/deploy.yml)

Food Totem is a web API application that controls the flow of customer orders and kitchen preparing. It receives orders from customers and sends them to the kitchen with endpoints. The application is built with C#. It uses a MySQL database to store the orders and the kitchen status. The application image is available on Docker Hub.

## How to Run

### Docker Compose
First of all, clone this project. Then, open the `src` folder and run the following command:

```cmd
docker-compose up
```

### Kubernetes
If you like to use the application with Kubernetes, you can go to `src` folder and run the following commands(in sequence):

```cmd
kubectl apply -f api-deployment.yaml
```

### Terraform

If you like to use the application with Terraform, that will provide the service and deployment on the `docker-desktop` cluster on Kubernetes (be aware that is necessary to install Docker Desktop and turn on the Kubernetes in the settings first). You can go to `src` folder and run the following command:

```cmd
terraform apply --auto-aprove
```

The application will be available on `http://localhost:8080/swagger/index.html`.

### Checkout

At this point, API integrates the payment method with Mercado Pago payment gateway. So, to do a checkout, you can use the following POST endpoint:

```api/Order```

on Swagger. This endpoint will create an order and will create a Mercado Pago QRCode payment on test environment. You can use QRCodeMonkey website on `Text` column to create the QRCode image. After that, you can use the Mercado Pago app to scan the QRCode.

(Maybe the scan returns some error because it is an test environment. I tested with my own Mercado Pago test accounts.)

After that, using the endpoint ```api/Order/queued``` you can see the orders in the kitchen queue.

