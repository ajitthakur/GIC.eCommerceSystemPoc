This project demonstrates microservices — User Service and Order Service — that communicate via Apache Kafka running in Docker.
- **User Service**: Manages user registration and information
- **Order Service**: Handles order creation and management

**1. Prerequisites**
	Docker Desktop installed
	.NET 8 SDK installed

**2. Run application Service**
 Open Terminal and run below command **docker-compose up -build**
 This will start Kafka, User Service and Order Service as below port
 User Service apps → localhost:5001
 Order Service apps → localhost:5001
 Container Kafka apps → kafka:29092

 Open Swagger for all Services Application to perfrom **Test** on API
 http://localhost:5001/swagger/
 http://localhost:5002/swagger/

**3. Workflow**
User Service - Create User and publishes UserCreated events to Kafka.
Order Service -  consumes users topic to react to new users.
Order Service - Create Order And publishes OrderCreated events to Kafka
Other services can consume orders topic for downstream processing.
