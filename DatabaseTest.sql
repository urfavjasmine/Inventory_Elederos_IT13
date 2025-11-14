-- Test script to verify database and stored procedures exist
USE DB_Inventory_Elederos_IT13;
GO

-- Check if database exists
IF DB_ID('DB_Inventory_Elederos_IT13') IS NULL
BEGIN
    PRINT '❌ Database DB_Inventory_Elederos_IT13 does not exist';
END
ELSE
BEGIN
    PRINT '✅ Database DB_Inventory_Elederos_IT13 exists';
END
GO

-- Check if tables exist
IF OBJECT_ID('Products', 'U') IS NOT NULL
BEGIN
    PRINT '✅ Products table exists';
END
ELSE
BEGIN
    PRINT '❌ Products table does not exist';
END
GO

IF OBJECT_ID('Suppliers', 'U') IS NOT NULL
BEGIN
    PRINT '✅ Suppliers table exists';
END
ELSE
BEGIN
    PRINT '❌ Suppliers table does not exist';
END
GO

-- Check if stored procedures exist
IF OBJECT_ID('sp_GetAllProducts', 'P') IS NOT NULL
BEGIN
    PRINT '✅ sp_GetAllProducts stored procedure exists';
END
ELSE
BEGIN
    PRINT '❌ sp_GetAllProducts stored procedure does not exist';
END
GO

IF OBJECT_ID('sp_GetAllSuppliers', 'P') IS NOT NULL
BEGIN
    PRINT '✅ sp_GetAllSuppliers stored procedure exists';
END
ELSE
BEGIN
    PRINT '❌ sp_GetAllSuppliers stored procedure does not exist';
END
GO

-- Test the stored procedures
PRINT ''
PRINT 'Testing stored procedures...';
GO

-- Test sp_GetAllProducts
BEGIN TRY
    EXEC sp_GetAllProducts;
    PRINT '✅ sp_GetAllProducts executed successfully';
END TRY
BEGIN CATCH
    PRINT '❌ sp_GetAllProducts failed: ' + ERROR_MESSAGE();
END CATCH
GO

-- Test sp_GetAllSuppliers
BEGIN TRY
    EXEC sp_GetAllSuppliers;
    PRINT '✅ sp_GetAllSuppliers executed successfully';
END TRY
BEGIN CATCH
    PRINT '❌ sp_GetAllSuppliers failed: ' + ERROR_MESSAGE();
END CATCH
GO

PRINT ''
PRINT 'Database test completed.';
