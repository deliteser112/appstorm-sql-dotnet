# Products API

This project is a simple ASP.NET Core Web API for managing products with basic CRUD operations. It includes SQL queries for data retrieval and can be tested online using tools like db-fiddle. You can check all of them at [this video](https://www.loom.com/share/8f754452e6c9453c8d709ca927a1a9fc?sid=b3c09958-dfcd-4926-b50f-e2689f822f4c)

## Getting Started

Follow these steps to set up the API locally.

### Prerequisites

- .NET SDK 7.0 or 8.0 (Preview)
- Visual Studio or any .NET-supported IDE
- Entity Framework Core (In-Memory provider for testing)
- SQL Server or an online SQL tool like db-fiddle

### Running the API

1. **Clone the repository:**

   ```bash
   git clone https://github.com/deliteser112/appstorm-sql-dotnet
   ```

2. **Navigate to the project folder:**

   ```bash
   cd appstorm-sql-dotnet
   ```

3. **Restore dependencies and build the project:**

   ```bash
   dotnet restore
   dotnet build
   ```

4. **Run the project:**

   ```bash
   dotnet run
   ```

   The API will be accessible at [http://localhost:5273](http://localhost:5273).

## API Endpoints

| Method | Endpoint         | Description                      |
|--------|------------------|----------------------------------|
| GET    | /products        | Retrieves all products           |
| GET    | /products/{id}   | Retrieves a product by ID        |
| POST   | /products        | Creates a new product            |
| PUT    | /products/{id}   | Updates an existing product by ID|
| DELETE | /products/{id}   | Deletes a product by ID          |

### Example Request for Creating a Product

```http
POST /products
Content-Type: application/json

{
    "name": "New Product",
    "price": 199.99,
    "stock": 25
}
```

## Testing

This project includes unit tests for the `ProductsController`. Run the tests with:

```bash
dotnet test
```

Ensure all dependencies are installed and the project builds successfully before running the tests.

## SQL Queries

Test these SQL queries using an online tool like db-fiddle.

1. **Retrieve Products Based on Price and Stock**

   ```sql
   SELECT * 
   FROM Products 
   WHERE Price > 100 
     AND Stock < 50;
   ```

2. **Join Two Tables (Orders and Customers)**

   ```sql
   SELECT Customers.CustomerName, Orders.OrderID, Orders.OrderDate 
   FROM Orders
   JOIN Customers ON Orders.CustomerID = Customers.CustomerID
   WHERE Customers.City = 'New York';
   ```

3. **Total Sales for Each Product**

   ```sql
   SELECT ProductID, SUM(TotalAmount) AS TotalSales
   FROM Sales
   GROUP BY ProductID;
   ```

4. **Find Frequent Customers**

   ```sql
   SELECT Customers.CustomerName
   FROM Orders
   JOIN Customers ON Orders.CustomerID = Customers.CustomerID
   GROUP BY Customers.CustomerID, Customers.CustomerName
   HAVING COUNT(Orders.OrderID) > 2;
   ```

### SQL Testing Sample Data

Use the following schema and data to test the SQL queries.

1. **Products Table and Sample Data:**

   ```sql
   CREATE TABLE Products (
       ProductID INT PRIMARY KEY,
       ProductName VARCHAR(100),
       Price DECIMAL(10, 2),
       Stock INT
   );

   INSERT INTO Products (ProductID, ProductName, Price, Stock)
   VALUES (1, 'Product A', 150, 30),
          (2, 'Product B', 90, 60),
          (3, 'Product C', 120, 40);
   ```

2. **Customers Table and Sample Data:**

   ```sql
   CREATE TABLE Customers (
       CustomerID INT PRIMARY KEY,
       CustomerName VARCHAR(100),
       City VARCHAR(100)
   );

   INSERT INTO Customers (CustomerID, CustomerName, City)
   VALUES (1, 'John Doe', 'New York'),
          (2, 'Jane Smith', 'Los Angeles'),
          (3, 'Mark Lee', 'New York');
   ```

3. **Orders Table and Sample Data:**

   ```sql
   CREATE TABLE Orders (
       OrderID INT PRIMARY KEY,
       CustomerID INT,
       OrderDate DATE,
       FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
   );

   INSERT INTO Orders (OrderID, CustomerID, OrderDate)
   VALUES (1, 1, '2023-09-04'),
          (2, 2, '2023-09-05'),
          (3, 1, '2023-09-05'),
          (4, 3, '2023-09-05'),
          (5, 1, '2023-09-05'),
          (6, 2, '2023-09-06'),
          (7, 2, '2023-09-07'),
          (8, 1, '2023-09-08'),
          (9, 3, '2023-09-09');
   ```

4. **Sales Table and Sample Data:**

   ```sql
   CREATE TABLE Sales (
       SaleID INT PRIMARY KEY,
       ProductID INT,
       TotalAmount DECIMAL(10, 2),
       FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
   );

   INSERT INTO Sales (SaleID, ProductID, TotalAmount)
   VALUES (1, 1, 500.00),
          (2, 2, 300.00),
          (3, 3, 700.00);
   ```
