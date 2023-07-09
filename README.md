# Food Totem
Food Totem is a web API application that controls the flow of customer orders and kitchen preparing. It receives orders from customers and sends them to the kitchen with endpoints. The application is built with C#. It uses a MySQL database to store the orders and the kitchen status. The application image is available on Docker Hub.

## How to Run

First of all, clone this project. Then, open the `src` folder and run the following command:

```docker-compose up```

The application will be available on `http://localhost:8080/swagger/index.html`.

## Observations

While running the docker-compose command, the application could show in the first seconds some errors. That's because MySQL is still initializing. Wait a few seconds and then the application will be ready to use by itself.