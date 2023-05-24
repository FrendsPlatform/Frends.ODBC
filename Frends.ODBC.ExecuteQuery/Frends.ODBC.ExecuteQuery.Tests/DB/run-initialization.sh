#!/bin/bash
sleep 30s

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P yourStrong!Password -d master -i init-db.sql