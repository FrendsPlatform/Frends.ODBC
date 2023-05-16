$osName = $PSVersionTable.OS
Write-Host "The current operating system is: $osName"

if ([Environment]::Is64BitOperatingSystem) {
	Write-Host "Get file for $osName"
    Invoke-WebRequest -Uri "https://go.microsoft.com/fwlink/?linkid=2223304" -OutFile "$PWD/odbc.msi"
	Write-Host "File for $osName downloaded"
	Start-Process -FilePath "$PWD/odbc.msi" -ArgumentList "/qr IACCEPTMSODBCSQLLICENSETERMS=YES"
	Write-Host "Installation complete"
	Add-OdbcDsn -Name "ODBC_testDB" -DriverName "ODBC Driver 17 for SQL Server" -DsnType "User" -Platform "64-bit" -SetPropertyValue @("Name=ODBC_testDB", "Server=localhost", "Trusted_Connection=Yes", "Database=UnitTests")
	Write-Host "ODBC_testDB.accdb added"
}
else {
	Write-Host "Get file for $osName"
    Invoke-WebRequest -Uri "https://go.microsoft.com/fwlink/?linkid=2223303" -OutFile "$PWD/odbc.msi"
	Write-Host "File for $osName downloaded"
	Start-Process -FilePath "$PWD/odbc.msi" -ArgumentList "/qr IACCEPTMSODBCSQLLICENSETERMS=YES"
	Write-Host "Installation complete"
	Add-OdbcDsn -Name "ODBC_testDB" -DriverName "ODBC Driver 17 for SQL Server" -DsnType "User" -Platform "32-bit" -SetPropertyValue @("Name=ODBC_testDB", "Server=localhost", "Trusted_Connection=Yes", "Database=UnitTests")
	Write-Host "ODBC_testDB_32.mdb added"
}