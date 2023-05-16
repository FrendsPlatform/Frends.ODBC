$osName = $PSVersionTable.OS
Write-Host "The current operating system is: $osName"

if ([Environment]::Is64BitOperatingSystem) {
	Write-Host "Get file for $osName"
    Invoke-WebRequest -Uri "https://go.microsoft.com/fwlink/?linkid=2223304" -OutFile "$PWD/odbc.msi"
	Write-Host "File for $osName downloaded"
	Start-Process -FilePath "$PWD/odbc.msi" -ArgumentList "/qr IACCEPTMSODBCSQLLICENSETERMS=YES"
	Write-Host "Installation complete"
	Add-OdbcDsn -Name "ODBC_testDB" -DriverName "Microsoft Access Driver (*.mdb, *.accdb)" -DsnType "User" -Platform "64-bit" -SetPropertyValue 'Dbq=.\Frends.ODBC.ExecuteQuery.Tests\TestFiles\ODBC_testDB.accdb'
	Write-Host "ODBC_testDB.accdb added"
}
else {
	Write-Host "Get file for $osName"
    Invoke-WebRequest -Uri "https://go.microsoft.com/fwlink/?linkid=2223303" -OutFile "$PWD/odbc.msi"
	Write-Host "File for $osName downloaded"
	Start-Process -FilePath "$PWD/odbc.msi" -ArgumentList "/qr IACCEPTMSODBCSQLLICENSETERMS=YES"
	Write-Host "Installation complete"
	Add-OdbcDsn -Name "ODBC_testDB" -DriverName "Microsoft Access Driver (*.mdb, *.accdb)" -DsnType "User" -Platform "32-bit" -SetPropertyValue 'Dbq=.\Frends.ODBC.ExecuteQuery.Tests\TestFiles\ODBC_testDB_32.mdb'
	Write-Host "ODBC_testDB_32.mdb added"
}