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

On the root folder:

```cmd 
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

You can check the repositories of the microsservices on the links below:

- [Food Totem Demand](https://github.com/leonardo-avila/food-totem-demand)
- [Food Totem Payment](https://github.com/leonardo-avila/food-totem-payment)
- [Food Totem Catalog](https://github.com/leonardo-avila/food-totem-catalog)

## Architecture

The Food Totem is written following the Clean Architecture and Microservices. At this point, the communication is centered on the Food Totem API and uses HTTP requests to get information between services. On the future months, it will apply the SAGA pattern to add resilience. By now, this is the service design:

![image](https://github.com/leonardo-avila/food-totem/assets/29763488/daa543b7-71f5-41c8-b441-687c4b558332)

And the AWS deployment that Terraform generates is:

![foodtotem drawio](https://github.com/leonardo-avila/food-totem/assets/29763488/30a8af03-3061-4397-9219-1acf90692bdd)
