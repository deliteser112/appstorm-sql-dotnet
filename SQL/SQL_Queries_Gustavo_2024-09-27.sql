
-- 1. Retrieve Products Based on Price and Stock
SELECT *
FROM Products
WHERE Price > 100 AND Stock < 50;

-- 2. Join Two Tables Using Orders and Customers
SELECT Customers.CustomerName, Orders.OrderID, Orders.OrderDate
FROM Orders
JOIN Customers ON Orders.CustomerID = Customers.CustomerID
WHERE Customers.City = 'New York';

-- 3. Total Sales for Each Product
SELECT ProductID, SUM(TotalAmount) AS TotalSales
FROM Sales
GROUP BY ProductID;

-- 4. Find Frequent Customers
SELECT Customers.CustomerName
FROM Orders
JOIN Customers ON Orders.CustomerID = Customers.CustomerID
GROUP BY Customers.CustomerID, Customers.CustomerName
HAVING COUNT(Orders.OrderID) > 2;
