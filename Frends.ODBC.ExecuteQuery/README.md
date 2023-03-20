# Frends.ODBC.ExecuteQuery
Frends Task to execute ODBC query.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT) 
[![Build](https://github.com/FrendsPlatform/Frends.ODBC/actions/workflows/ExecuteQuery_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.ODBC/actions)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.ODBC/Frends.ODBC.ExecuteQuery|main)

# Installing

You can install the Task via Frends UI Task View.

## Building


Rebuild the project

`dotnet build`

Run tests

`Add-OdbcDsn -Name "ODBC_testDB" -DriverName "Microsoft Access Driver (*.mdb, *.accdb)" -DsnType "User" -Platform "64-bit" -SetPropertyValue "Dbq=$pwd\TestFiles\ODBC_testDB.accdb"`

`dotnet test`

After tests:


Create a NuGet package

`dotnet pack --configuration Release`
