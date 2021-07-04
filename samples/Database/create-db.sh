# Wait an arbitrary 60 seconds for SQL Server to warm up
sleep 60s

# Create the database and tables. Note: Use the password defined in the Dockerfile
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P PASSWORD12345! -d master -i Lion.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P PASSWORD12345! -d Testing -i Namespaces.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P PASSWORD12345! -d Testing -i Bundles.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P PASSWORD12345! -d Testing -i Messages.sql