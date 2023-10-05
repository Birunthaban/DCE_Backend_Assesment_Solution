-- Create the OnlineRetailDB database
CREATE DATABASE OnlineRetailDB;

-- Use the OnlineRetailDB database
USE OnlineRetailDB;

-- Create the Supplier Table
CREATE TABLE Supplier (
    SupplierId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SupplierName NVARCHAR(50) NOT NULL,
    CreatedOn DATETIME DEFAULT GETDATE() NOT NULL,
    IsActive BIT DEFAULT 1 NOT NULL
);

-- Create the Product Table
CREATE TABLE Product (
    ProductId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProductName NVARCHAR(50) NOT NULL,
    UnitPrice DECIMAL(18, 2) NOT NULL,
    SupplierId UNIQUEIDENTIFIER DEFAULT NEWID(),
    CreatedOn DATETIME DEFAULT GETDATE() NOT NULL,
    IsActive BIT DEFAULT 1 NOT NULL,
    FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);

-- Create the Customer Table
CREATE TABLE Customer (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(30) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    FirstName NVARCHAR(20) NOT NULL,
    LastName NVARCHAR(20) NOT NULL,
    CreatedOn DATETIME DEFAULT GETDATE() NOT NULL,
    IsActive BIT DEFAULT 1 NOT NULL
);

-- Create the Order Table
CREATE TABLE [Order] (
    OrderId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ProductId UNIQUEIDENTIFIER DEFAULT NEWID(),
    OrderStatus INT NOT NULL,
    OrderType INT NOT NULL,
    OrderBy UNIQUEIDENTIFIER DEFAULT NEWID(),
    OrderedOn DATETIME DEFAULT GETDATE() NOT NULL,
    ShippedOn DATETIME,
    IsActive BIT DEFAULT 1 NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    FOREIGN KEY (OrderBy) REFERENCES Customer(UserId)
);

CREATE PROCEDURE GetActiveOrdersByCustomer
    @CustomerId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT
        [Order].OrderId,
        [Order].OrderStatus,
        [Order].OrderType,
        [Order].OrderedOn,
        [Order].ShippedOn,
        Product.ProductId, -- Include ProductId
        Supplier.SupplierId, -- Include SupplierId
        Supplier.SupplierName,
        Product.ProductName,
        Product.UnitPrice
    FROM
        [Order]
    JOIN
        Product ON [Order].ProductId = Product.ProductId
    JOIN
        Supplier ON Product.SupplierId = Supplier.SupplierId
    WHERE
        [Order].OrderBy = @CustomerId
        AND [Order].IsActive = 1
END
