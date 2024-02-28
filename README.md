# Food Totem
[![Build](https://github.com/leonardo-avila/food-totem/actions/workflows/build.yml/badge.svg)](https://github.com/leonardo-avila/food-totem/actions/workflows/build.yml)
[![Deploy](https://github.com/leonardo-avila/food-totem/actions/workflows/deploy.yml/badge.svg)](https://github.com/leonardo-avila/food-totem/actions/workflows/deploy.yml)

Food Totem is a web API (gateway) application that controls the customer orders and kitchen workflow. It receives orders from customers and sends them to the kitchen with endpoints. It also validates the JWT token of the user to approve the requests. The application is built with C#. It uses a MySQL and MongoDB databases to store the orders and the kitchen status. The application image is available on Docker Hub.

## Requisites
- .NET 6.0 or above
- Docker
- Kubernetes
- Terraform

## How to Run

You can check the repositories of the microsservices on the links below. Start all of them before run this project:
- [JWT Lambda](https://github.com/leonardo-avila/jwt-lambda) - Deploy the lambda to AWS to use his API Gateway as authorization url.
- [Food Totem Demand](https://github.com/leonardo-avila/food-totem-demand)
- [Food Totem Payment](https://github.com/leonardo-avila/food-totem-payment)
- [Food Totem Catalog](https://github.com/leonardo-avila/food-totem-catalog)

In `docker-compose.yaml` file on `infra/local` folder, set the JWT Issuer Signing Key and the AuthorizationUrl. Both should be equal to the used on JWT Lambda. Then, you can run on the root folder:

```bash
make run-services
```


The following services and addresses will be available:
| **Service**    | **Address**           |
|----------------|-----------------------|
| Food Totem API | http://localhost:3000 |
| Demand API     | http://localhost:3001 |
| Payment API    | http://localhost:3002 |
| Catalog API    | http://localhost:3003 |

Make sure that the ports 3000 to 3003 are available before running the services.

## Architecture

The Food Totem is written following the Clean Architecture and Microservices. At this point, the communication is centered on the Food Totem API and uses HTTP requests to get information between services. On the future months, it will apply the SAGA pattern to add resilience. By now, this is the service design:

![image](https://github.com/leonardo-avila/food-totem/assets/29763488/77f3590d-94c7-48e4-b529-86d0dc00f4b9)

Simplified architecture diagram:
![image](https://github.com/leonardo-avila/food-totem/assets/29763488/2f5e75ef-5072-4102-8ac3-f5b1c7fb6ac3)

The Food Totem base service is provided by a load balancer with a public IP, while the other services are internal to the VPC. The base service performs JWT authentication and authorizes requests using the generated token. All requests are directed to the microservices that best serve them. Communication between microservices is done through messages in queues in RabbitMQ, and all notifications to clients are done via email, using the Mailtrap mock.

## SAGA Pattern - Coreography

Food Totem uses RabbitMQ as his coreographer to send messages between services. This choice was made considering the simplicity of implementation and also the few scenarios that needed to be covered by the messages. The messages are sent to queues for every event that happens on the system. The messages are sent to the queues and the services are listening to them as follow:

| **Queue**               | **Description**                                                                                         |
|-------------------------|---------------------------------------------------------------------------------------------------------|
| order-created-event     | Order was created and the payment service consume this message to generate the payment.                 |
| payment-failure-event   | There was a problem generating the problem. The Demand service consume this message deleting the order. |
| payment-paid-event      | The payment was concluded. Demand service consume the message updating the order.                       |
| payment-cancelled-event | Payment was cancelled. Demand service consume the message cancelling the order.                         |
| order-updated-event     | Every update on the order. Food Totem consume the message notifying the customer.                       |

## Customer notification

Since this is a study project and the main goal is to learn about software architecture with low or no cost, the customer notification is made by email using [Mailtrap](https://mailtrap.io/). This service uses a fake inbox to receive the emails and show them on the web interface. The email is sent by the Food Totem API when the order is updated. The email is sent to the customer with the order status.