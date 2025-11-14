# Database Setup Script for SmartStock Inventory System
Write-Host "Setting up SQL Server database..." -ForegroundColor Cyan
Write-Host ""

# Try multiple SQL Server instances
$instances = @(
    "localhost\SQLEXPRESS",
    "localhost", 
    ".\SQLEXPRESS",
    ".",
    "(local)\SQLEXPRESS",
    "(local)"
)

$scriptPath = Join-Path $PSScriptRoot "Database\CreateDatabase.sql"

foreach ($instance in $instances) {
    Write-Host "Trying to connect to: $instance" -ForegroundColor Yellow
    
    try {
        # Test connection
        $connectionTest = sqlcmd -S $instance -E -Q "SELECT 1" -h -1
        
        if ($connectionTest -eq "1") {
            Write-Host "✅ Connected successfully to $instance" -ForegroundColor Green
            Write-Host "Creating database and stored procedures..." -ForegroundColor Yellow
            
            # Execute the database script
            $result = sqlcmd -S $instance -E -i $scriptPath
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host ""
                Write-Host "🎉 Database setup completed successfully!" -ForegroundColor Green
                Write-Host "Database 'DB_Inventory_Elederos_IT13' and all stored procedures have been created." -ForegroundColor Green
                Write-Host ""
                Write-Host "Connection string to use:" -ForegroundColor Cyan
                Write-Host "Server=$instance;Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;" -ForegroundColor White
                exit 0
            }
        }
    }
    catch {
        Write-Host "❌ Failed to connect to $instance" -ForegroundColor Red
        continue
    }
}

Write-Host ""
Write-Host "❌ Could not connect to any SQL Server instance" -ForegroundColor Red
Write-Host ""
Write-Host "Please ensure:" -ForegroundColor Yellow
Write-Host "1. SQL Server is installed and running" -ForegroundColor White
Write-Host "2. SQL Server Browser service is started" -ForegroundColor White
Write-Host "3. Windows Authentication is enabled" -ForegroundColor White
Write-Host "4. TCP/IP and Named Pipes are enabled in SQL Server Configuration Manager" -ForegroundColor White
Write-Host ""
Write-Host "Manual setup:" -ForegroundColor Yellow
Write-Host "1. Open SQL Server Management Studio (SSMS)" -ForegroundColor White
Write-Host "2. Connect to your SQL Server instance" -ForegroundColor White
Write-Host "3. Open and execute: Database\CreateDatabase.sql" -ForegroundColor White

Read-Host "Press Enter to exit"
