# Food Totem
[![Build](https://github.com/leonardo-avila/food-totem/actions/workflows/build.yml/badge.svg)](https://github.com/leonardo-avila/food-totem/actions/workflows/build.yml)

Food Totem is a web API application that controls the flow of customer orders and kitchen preparing. It receives orders from customers and sends them to the kitchen with endpoints. The application is built with C#. It uses a MySQL database to store the orders and the kitchen status. The application image is available on Docker Hub.

## How to Run

First of all, clone this project. Then, open the `src` folder and run the following command:

```cmd
docker-compose up
```

If you like to use the application with Kubernetes, you can go to `src` folder and run the following commands(in sequence):

```cmd
kubectl apply -f db-deployment.yaml
```

```cmd
kubectl apply -f api-deployment.yaml
```

The application will be available on `http://localhost:8080/swagger/index.html`.

### Checkout

At this point, API integrates the payment method with Mercado Pago payment gateway. So, to do a checkout, you can use the following POST endpoint:

```api/Order```

on Swagger. This endpoint will create an order and will create a Mercado Pago QRCode payment on test environment. You can use QRCodeMonkey website on `Text` column to create the QRCode image. After that, you can use the Mercado Pago app to scan the QRCode.

(Maybe the scan returns some error because it is an test environment. I tested with my own Mercado Pago test accounts.)

After that, using the endpoint ```api/Order/queued``` you can see the orders in the kitchen queue.

## Observations

While running the docker-compose or api-deployment.yaml kubectl apply command, the application could show in the first seconds some errors. That's because MySQL is still initializing. Wait a few seconds and then the application will be ready to use by itself.
