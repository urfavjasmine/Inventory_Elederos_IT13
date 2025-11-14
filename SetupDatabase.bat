@echo off
echo Setting up SQL Server database...
echo.

REM Try to connect to SQL Server and execute the database script
sqlcmd -S localhost\SQLEXPRESS -E -i "Database\CreateDatabase.sql"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✅ Database setup completed successfully!
    echo Database 'DB_Inventory_Elederos_IT13' and all stored procedures have been created.
) else (
    echo.
    echo ❌ Failed to connect to SQL Server Express.
    echo.
    echo Please ensure:
    echo 1. SQL Server Express is installed and running
    echo 2. Windows Authentication is enabled
    echo 3. You have permission to create databases
    echo.
    echo Alternative: Run the script manually in SQL Server Management Studio:
    echo    1. Open SSMS
    echo    2. Connect to your SQL Server instance
    echo    3. Open and execute Database\CreateDatabase.sql
)

pause
