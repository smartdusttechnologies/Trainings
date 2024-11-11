#!/bin/bash
set -e

# Apply migrations to SQL Server context
echo "Applying migrations for SQL Server context..."
dotnet ef database update --context SqlServerDbContext

# Apply migrations to MySQL context
echo "Applying migrations for MySQL context..."
dotnet ef database update --context MySqlDbContext

# Start the application
echo "Starting the application..."
exec dotnet MultiDatabase.dll  # Replace YourApp.dll with the actual DLL name
