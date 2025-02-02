#!/bin/bash
set -e

# Wait for the database to be ready
echo "Waiting for PostgreSQL to be ready..."
/wait-for-it.sh postgres:5432 --timeout=15 -- echo "PostgreSQL is ready!"

# Start the application
echo "Starting the application..."
exec dotnet /app/WebAPI.API.dll
