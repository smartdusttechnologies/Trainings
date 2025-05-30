# Microservices Architecture Overview

## Introduction

This project follows a **Microservices Architecture**, where each service is independently developed, deployed, and managed. Below is a detailed explanation of each microservice, its responsibilities, technologies used, and API endpoints.

## Tech Stack

- **Backend:** ASP.NET Core
- **Frontend:** ASP.NET Core Razor Page
- **Database:** SQL Server, PostgreSQL, Redis
- **Pattern and Principles:**
  - **Architecture:** Vertical Slice Architecture, Clean Architecture
  - **Message Broker:** RabbitMQ
  - **Containerization:** Docker
  - **API Gateway:** YARP

# Microservices Overview

## Catalog Service (Product Service)

### Description:

Manages Product Addition, deletion, updates, retrieval by ID, and category , retrieval with pagination.

### Technology Stack:

- **Database:** Sql Server
- **Libraries:**
  - Entity Framework Core
  - Carter (for Minimal API endpoint definition)
- **Architecture:** Vertical Slice Architecture
- **Pattern and Principles:** CQRS, Pipeline Behavior with FluentValidation

### Key Features:

- Product Addition
- Deletion
- Update
- Get with pagination (PageIndex and PageSize)
- Get Product by ID
- Get Product by Category
- Health Check

### API Endpoints:

```http
GET /products?pageNumber=1&pageSize=5       # Get with pagination
GET /health                                 # Health check
GET /products/category/Fruits               # Get by Category
GET /products/{id}                          # Get Product by ID
POST /products                              # Insert a Product
PUT /products                               # Update Product
DELETE /products/{id}                       # Delete Product
```

## Order Service

### Description:

Processes customer orders and manages order history.

### Technology Stack:

- **Database:** SQL Server
- **Architecture:** Clean Architecture, Domain Driven Design

### Key Features:

- Order Addition
- Deletion
- Update
- Get with pagination (PageIndex and PageSize)
- Get Product by Name
- Get Product by CustomerId
- Health Check

### API Endpoints:

```http
GET /orders?pageNumber=1&pageSize=5         # Get with pagination
GET /health                                 # Health check
GET /order/customer/{customerID}            # Get by CustomerId
GET /order/{Ordername}                      # Get Basket by Ordername
POST /order                                 # Store Order
PUT /order                                  # Update Order
DELETE /order/{id}                          # Delete Order
```

## Discount Management

### Description :

ASP.NET Grpc Server application
Build a Highly Performant inter-service gRPC Communication with Basket Microservice
Exposing Grpc Services with creating Protobuf messages

### Technology Stack:

- **Database:** SQLite
- **Libraries:**
  - Entity Framework Core
  - Carter (for Minimal API endpoint definition)

### Key Features:

- Discount Addition
- Deletion
- Update
- Get by ProductName

### API Endpoints:

```http
GET /discount/{Productname}                   # Get Discount by Productname
POST /discount                                # Add Discount
POST /discount/checkout                       # Discount CheckOut
PUT /discount                                 # Update Discount
DELETE /discount/{id}                         # Delete Discount
```

## Basket Service

### Description:

Manages basket addition, deletion, updates, retrieval by ID, and Custommer retrieval by Pagination .

### Technology Stack:

- **Database:** Sql Server
- **Libraries:**
  - Entity Framework Core
  - Carter (for Minimal API endpoint definition)
- **Architecture:** Vertical Slice Architecture
- **Pattern and Principles:** CQRS, Pipeline Behavior with FluentValidation

### Key Features:

- Basket Addition
- Deletion
- Update
- Get with pagination (PageIndex and PageSize)
- Get Product by Name
- Get Product by Customer
- Health Check
- Connect with discount services using grpc

### API Endpoints:

```http
GET /basket?pageNumber=1&pageSize=5         # Get with pagination
GET /health                                 # Health check
GET /basket/customer/{id}                   # Get by CustomerId
GET /basket/{name}                          # Get Basket by Name
POST /basket                                # Store basket
POST /basket/checkout                       # Basket CheckOut
PUT /basket                                 # Update Basket
DELETE /basket/{id}                         # Delete Basket
```

## Microservices Communication

| Service          | Communication   | Technology       |
| ---------------- | --------------- | ---------------- |
| Product Service  | REST API        | HTTP             |
| Order Service    | Event-Driven    | RabbitMQ         |
| Basket Service   | REST API + gRPC | HTTP + gRPC      |
| Discount Service | Grpc            | Protocol Buffers |

## 🛠️ How to Run the Microservices?

## Prerequisites

Ensure you have the following installed on your system before proceeding:

- **Git** - [Download Git](https://git-scm.com/downloads)
- **Visual Studio 2022** (with .NET Core Workloads) - [Download Visual Studio](https://visualstudio.microsoft.com/)
- **Docker Desktop** - [Download Docker](https://www.docker.com/products/docker-desktop/)
- **SQL Server** (if using SQL-based databases) - [Download SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- **SSMS (SQL Server Management Studio)** - [Download SSMS](https://aka.ms/ssmsfullsetup)

---

### Clone the Repository:

```sh
git clone https://github.com/smartdusttechnologies/Trainings.git
cd Muskan/Microservices2
```

## Change the Connection String of All Microservices

### **For Basket Service**

```json
"ConnectionStrings": {
  "SqlServerDb": "Server=YOUR_SERVER;Database=BasketDb;User Id=YOUR_ID;Password=YOUR_PASSWORD;TrustServerCertificate=True",
  "Redis": "localhost:6379"
}
```

### **For Catalog Service**

```json
"ConnectionStrings": {
  "SqlServerDb": "Server=YOUR_SERVER;Database=CatalogDb;User Id=YOUR_ID;Password=YOUR_PASSWORD;TrustServerCertificate=True"
}
```

### **For Order Service**

```json
"ConnectionStrings": {
  "SqlServerDb": "Server=YOUR_SERVER;Database=OrderDb;User Id=YOUR_ID;Password=YOUR_PASSWORD;TrustServerCertificate=True"
}
```

### **For Logging Service**

```json
"ConnectionStrings": {
  "SqlServerDb": "Server=YOUR_SERVER;Database=OrderDb;User Id=YOUR_ID;Password=YOUR_PASSWORD;TrustServerCertificate=True"
}
```

---

- Database Migration (If Necessary)

Note:_ Migration is already run. Simply running the app will automatically update the database by checking if it exists. However, if needed, run the following commands:_

### **For Basket Service**

Navigate to each services

```sh
cd Microservices2/Basket/src
```

```sh
Add-Migration Initial -StartupProject Basket.API
```

### **For Catalog Service**

```sh
cd Microservices2/Product/src
```

```sh
Add-Migration Initial -StartupProject Catalog.API
```

### **For Order Service**

```sh
cd Microservices2/Order/src
```

```sh
Add-Migration Initial -StartupProject Ordering.API -Project Ordering.Infrastructure -OutDir Data/Migrations
```

```sh
cd Microservices2/Logging/src/Logging.API
```

```sh
Add-Migration Initial -StartupProject Logging.API
```

---

## Running the Microservices

### Run Docker Containers for the First Time (Visual Studio)

Open Visual Studio.

In the Startup Project dropdown, select Docker-compose.

Run the application.

Check it locally in Visual Studio.

### Run Docker Containers:

Ensure Docker Desktop is running, then execute the following command:

Go to each Microservices Solution folder for each

```sh
docker-compose up -d
```

In root project Microservices2
there is common images of database, messagebroker etc .,

```sh
docker-compose docker-compose.common.yml up -d
```

Alternative
Locate DockerComposeBat.bat.

Double-click to execute the script.

---

### Access API Gateway:

Once the application is running, you can access the API Gateway at:

```
https://localhost:6065/
```
