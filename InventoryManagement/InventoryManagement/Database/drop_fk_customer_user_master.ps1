# PowerShell script to drop the foreign key constraint from customer_master
# Usage: .\drop_fk_customer_user_master.ps1 -User root -Password "your_password" -Host localhost

param(
    [string]$User = "root",
    [string]$Password,
    [string]$Host = "localhost",
    [string]$Port = "3306",
    [string]$Database = "inventory_management",
    [string]$MySqlPath = "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe"
)

if (-not $Password) {
    Write-Host "Usage: .\drop_fk_customer_user_master.ps1 -User <user> -Password <password> [-Host <host>] [-Port <port>]" -ForegroundColor Yellow
    Write-Host "Example: .\drop_fk_customer_user_master.ps1 -User root -Password mypassword" -ForegroundColor Yellow
    exit 1
}

# Check if MySQL client exists
if (-not (Test-Path $MySqlPath)) {
    Write-Host "MySQL client not found at: $MySqlPath" -ForegroundColor Red
    Write-Host "Please install MySQL or update the MySqlPath parameter." -ForegroundColor Red
    exit 1
}

# Get the SQL script path
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$sqlFile = Join-Path $scriptDir "drop_fk_customer_user_master.sql"

if (-not (Test-Path $sqlFile)) {
    Write-Host "SQL script not found at: $sqlFile" -ForegroundColor Red
    exit 1
}

Write-Host "Executing SQL script to drop foreign key constraint..." -ForegroundColor Cyan
Write-Host "Database: $Database on $Host`:$Port" -ForegroundColor Cyan

# Execute the SQL script
& $MySqlPath -u $User -p"$Password" -h $Host -P $Port $Database < $sqlFile

if ($LASTEXITCODE -eq 0) {
    Write-Host "Foreign key constraint dropped successfully!" -ForegroundColor Green
} else {
    Write-Host "Error executing SQL script. Exit code: $LASTEXITCODE" -ForegroundColor Red
    exit 1
}
