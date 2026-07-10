$connString = 'Server=localhost; Database=inventory_management; Uid=root; Pwd=root;'
$dependencyPath = Join-Path $PSScriptRoot 'packages\System.Threading.Tasks.Extensions.4.5.4\lib\net461\System.Threading.Tasks.Extensions.dll'
$assemblyPath = Join-Path $PSScriptRoot 'packages\MySql.Data.9.7.0\lib\net462\MySql.Data.dll'

if (-not (Test-Path $dependencyPath)) {
    Write-Error "Dependency not found: $dependencyPath"
    exit 1
}

if (-not (Test-Path $assemblyPath)) {
    Write-Error "MySql.Data assembly not found: $assemblyPath"
    exit 1
}

Add-Type -Path $dependencyPath
Add-Type -Path $assemblyPath

$connection = New-Object MySql.Data.MySqlClient.MySqlConnection($connString)
$connection.Open()
$command = $connection.CreateCommand()
$command.CommandText = 'DESCRIBE customer_master'
$reader = $command.ExecuteReader()
while ($reader.Read()) {
    Write-Output ($reader.GetString(0) + ',' + $reader.GetString(1))
}
$reader.Close()
$connection.Close()
