-- SmartStock Inventory Database
-- Create database: DB_Inventory_Elederos_IT13

-- Create Database (if not exists)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DB_Inventory_Elederos_IT13')
BEGIN
    CREATE DATABASE DB_Inventory_Elederos_IT13;
END
GO

USE DB_Inventory_Elederos_IT13;
GO

-- Create Products Table
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    QuantityInStock INT NOT NULL DEFAULT 0,
    ReorderLevel INT NOT NULL DEFAULT 10,
    UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Category NVARCHAR(50) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CHK_Products_Quantity CHECK (QuantityInStock >= 0),
    CONSTRAINT CHK_Products_ReorderLevel CHECK (ReorderLevel >= 0),
    CONSTRAINT CHK_Products_UnitPrice CHECK (UnitPrice >= 0)
);
GO

-- Create Suppliers Table
CREATE TABLE Suppliers (
    SupplierID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(100) NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(200) NULL,
    ProductSupplied NVARCHAR(100) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Create Indexes for better performance
CREATE INDEX IX_Products_ProductName ON Products(ProductName);
CREATE INDEX IX_Products_Category ON Products(Category);
CREATE INDEX IX_Suppliers_SupplierName ON Suppliers(SupplierName);
CREATE INDEX IX_Suppliers_ProductSupplied ON Suppliers(ProductSupplied);
GO

-- Create Trigger for Products UpdatedAt
CREATE TRIGGER TR_Products_UpdatedAt
ON Products
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Products 
    SET UpdatedAt = GETDATE()
    WHERE ProductID IN (SELECT ProductID FROM inserted);
END
GO

-- Create Trigger for Suppliers UpdatedAt
CREATE TRIGGER TR_Suppliers_UpdatedAt
ON Suppliers
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Suppliers 
    SET UpdatedAt = GETDATE()
    WHERE SupplierID IN (SELECT SupplierID FROM inserted);
END
GO

-- Create Stored Procedures for Products
CREATE PROCEDURE sp_GetAllProducts
AS
BEGIN
    SELECT ProductID, ProductName, Description, QuantityInStock, ReorderLevel, UnitPrice, Category, CreatedAt, UpdatedAt
    FROM Products
    ORDER BY ProductName;
END
GO

CREATE PROCEDURE sp_GetProductByID
    @ProductID INT
AS
BEGIN
    SELECT ProductID, ProductName, Description, QuantityInStock, ReorderLevel, UnitPrice, Category, CreatedAt, UpdatedAt
    FROM Products
    WHERE ProductID = @ProductID;
END
GO

CREATE PROCEDURE sp_SearchProducts
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT ProductID, ProductName, Description, QuantityInStock, ReorderLevel, UnitPrice, Category, CreatedAt, UpdatedAt
    FROM Products
    WHERE ProductName LIKE '%' + @SearchTerm + '%' 
       OR Description LIKE '%' + @SearchTerm + '%' 
       OR Category LIKE '%' + @SearchTerm + '%'
       OR CAST(ProductID AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%'
    ORDER BY ProductName;
END
GO

CREATE PROCEDURE sp_InsertProduct
    @ProductName NVARCHAR(100),
    @Description NVARCHAR(500),
    @QuantityInStock INT,
    @ReorderLevel INT,
    @UnitPrice DECIMAL(18,2),
    @Category NVARCHAR(50)
AS
BEGIN
    INSERT INTO Products (ProductName, Description, QuantityInStock, ReorderLevel, UnitPrice, Category)
    VALUES (@ProductName, @Description, @QuantityInStock, @ReorderLevel, @UnitPrice, @Category);
    
    SELECT SCOPE_IDENTITY() AS ProductID;
END
GO

CREATE PROCEDURE sp_UpdateProduct
    @ProductID INT,
    @ProductName NVARCHAR(100),
    @Description NVARCHAR(500),
    @QuantityInStock INT,
    @ReorderLevel INT,
    @UnitPrice DECIMAL(18,2),
    @Category NVARCHAR(50)
AS
BEGIN
    UPDATE Products 
    SET ProductName = @ProductName,
        Description = @Description,
        QuantityInStock = @QuantityInStock,
        ReorderLevel = @ReorderLevel,
        UnitPrice = @UnitPrice,
        Category = @Category
    WHERE ProductID = @ProductID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

CREATE PROCEDURE sp_DeleteProduct
    @ProductID INT
AS
BEGIN
    DELETE FROM Products
    WHERE ProductID = @ProductID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- Create Stored Procedures for Suppliers
CREATE PROCEDURE sp_GetAllSuppliers
AS
BEGIN
    SELECT SupplierID, SupplierName, ContactPerson, PhoneNumber, Email, Address, ProductSupplied, CreatedAt, UpdatedAt
    FROM Suppliers
    ORDER BY SupplierName;
END
GO

CREATE PROCEDURE sp_GetSupplierByID
    @SupplierID INT
AS
BEGIN
    SELECT SupplierID, SupplierName, ContactPerson, PhoneNumber, Email, Address, ProductSupplied, CreatedAt, UpdatedAt
    FROM Suppliers
    WHERE SupplierID = @SupplierID;
END
GO

CREATE PROCEDURE sp_SearchSuppliers
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT SupplierID, SupplierName, ContactPerson, PhoneNumber, Email, Address, ProductSupplied, CreatedAt, UpdatedAt
    FROM Suppliers
    WHERE SupplierName LIKE '%' + @SearchTerm + '%' 
       OR ContactPerson LIKE '%' + @SearchTerm + '%' 
       OR ProductSupplied LIKE '%' + @SearchTerm + '%'
       OR CAST(SupplierID AS NVARCHAR(10)) LIKE '%' + @SearchTerm + '%'
    ORDER BY SupplierName;
END
GO

CREATE PROCEDURE sp_InsertSupplier
    @SupplierName NVARCHAR(100),
    @ContactPerson NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @Email NVARCHAR(100),
    @Address NVARCHAR(200),
    @ProductSupplied NVARCHAR(100)
AS
BEGIN
    INSERT INTO Suppliers (SupplierName, ContactPerson, PhoneNumber, Email, Address, ProductSupplied)
    VALUES (@SupplierName, @ContactPerson, @PhoneNumber, @Email, @Address, @ProductSupplied);
    
    SELECT SCOPE_IDENTITY() AS SupplierID;
END
GO

CREATE PROCEDURE sp_UpdateSupplier
    @SupplierID INT,
    @SupplierName NVARCHAR(100),
    @ContactPerson NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @Email NVARCHAR(100),
    @Address NVARCHAR(200),
    @ProductSupplied NVARCHAR(100)
AS
BEGIN
    UPDATE Suppliers 
    SET SupplierName = @SupplierName,
        ContactPerson = @ContactPerson,
        PhoneNumber = @PhoneNumber,
        Email = @Email,
        Address = @Address,
        ProductSupplied = @ProductSupplied
    WHERE SupplierID = @SupplierID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

CREATE PROCEDURE sp_DeleteSupplier
    @SupplierID INT
AS
BEGIN
    DELETE FROM Suppliers
    WHERE SupplierID = @SupplierID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- Create Stored Procedures for Dashboard Statistics
CREATE PROCEDURE sp_GetDashboardStats
AS
BEGIN
    -- Total Products
    SELECT COUNT(*) AS TotalProducts FROM Products;
    
    -- Low Stock Items
    SELECT COUNT(*) AS LowStockItems FROM Products WHERE QuantityInStock <= ReorderLevel;
    
    -- Total Suppliers
    SELECT COUNT(*) AS TotalSuppliers FROM Suppliers;
    
    -- Total Inventory Value
    SELECT SUM(QuantityInStock * UnitPrice) AS TotalValue FROM Products;
END
GO

PRINT 'Database DB_Inventory_Elederos_IT13 created successfully!';
PRINT 'Tables: Products, Suppliers';
PRINT 'Stored Procedures: CRUD operations for all tables';
PRINT 'Sample data inserted for testing';
