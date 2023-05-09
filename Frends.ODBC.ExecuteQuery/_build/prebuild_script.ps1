Set-ExecutionPolicy RemoteSigned

Get-OdbcDriver -Name "SQL Server*" -Platform "64-bit"

Add-OdbcDsn -Name "ODBC_testDB" -DriverName "SQL Server" -DsnType "User" -Platform "64-bit" -SetPropertyValue @("Name=ODBC_testDB", "Server=localhost", "Trusted_Connection=Yes", "Database=UnitTests")