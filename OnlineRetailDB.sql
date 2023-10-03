-- Create SQL Table Named Customer
CREATE TABLE Customer (
    UserId UNIQUEIDENTIFIER PRIMARY KEY,
    Username NVARCHAR(30),
    Email NVARCHAR(20),
    FirstName NVARCHAR(20),
    LastName NVARCHAR(20),
    CreatedOn DATETIME,
    IsActive BIT
);
-- Create SQL Table Named Supplier
CREATE TABLE Supplier (
    SupplierId UNIQUEIDENTIFIER PRIMARY KEY,
    SupplierName NVARCHAR(50),
    CreatedOn DATETIME,
    IsActive BIT
);
-- Create SQL Table Named Product
CREATE TABLE Product (
    ProductId UNIQUEIDENTIFIER PRIMARY KEY,
    ProductName NVARCHAR(50),
    UnitPrice DECIMAL,
    SupplierId UNIQUEIDENTIFIER,
    CreatedOn DATETIME,
    IsActive BIT,
    FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);
-- Create SQL Table Named [Order] 
CREATE TABLE [Order] (
    OrderId UNIQUEIDENTIFIER PRIMARY KEY,
    ProductId UNIQUEIDENTIFIER,
    OrderStatus INT,
    OrderType INT,
    OrderBy UNIQUEIDENTIFIER,
    OrderedOn DATETIME,
    ShippedOn DATETIME,
    IsActive BIT,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    FOREIGN KEY (OrderBy) REFERENCES Customer(UserId)
);
