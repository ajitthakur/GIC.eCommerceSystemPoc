This project demonstrates microservices — User Service and Order Service — that communicate via Apache Kafka running in Docker.
- **User Service**: Manages user registration and information
- **Order Service**: Handles order creation and management

**1. Prerequisites**
	Docker Desktop installed
	.NET 8 SDK installed

**2. Run application Service**
 ## Open Terminal and run below command **docker-compose up -build**
 ## This will start Kafka, User Service and Order Service as below port
 ## User Service apps → localhost:5001
 ## Order Service apps → localhost:5001
 ## Container Kafka apps → kafka:29092 (If connection seem broken, Please help to delete existing kafka & Zookeeper image)

 Open Swagger for all Services Application to perfrom **Test** on API
 http://localhost:5001/Index.html
 http://localhost:5002/Index.html

 **2.1 Itegration Test case added for User Service : Run Test Project on already created container from docker compose**
 ## Run Automated Test case from GIC.UserService.Test Project


**3. Workflow**
User Service - Create User and publishes UserCreated events to Kafka.
Order Service -  consumes users topic to react to new users.
Order Service - Create Order for user if created by UserService And publishes OrderCreated events to Kafka.
## Order Service - If User not avilable then it will show error message. (Comunication Between 2 service)
User Service - Consume Order events from Kafka and store for users.

**4. AI Prompt Used**
Alternative for MemoryCache
Kafka Configuartion with Docker

