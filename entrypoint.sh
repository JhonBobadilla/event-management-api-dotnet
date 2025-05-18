#!/bin/bash

# Esperar a PostgreSQL
echo "Waiting for PostgreSQL to be ready..."
while ! pg_isready -h db -U postgres -d event_management_db -t 1; do
  sleep 2
done

# Aplicar migraciones
echo "Checking for database migrations..."
export PATH="$PATH:/root/.dotnet/tools"

if [ $(find /app/migrations -name "*.cs" | wc -l) -gt 0 ]; then
  echo "Applying existing migrations..."
  dotnet ef database update --project /app/EventManagement.Presentation.dll
else
  echo "Creating initial migration..."
  dotnet ef migrations add InitialCreate --project /app/EventManagement.Presentation.dll --output-dir /app/migrations
  dotnet ef database update --project /app/EventManagement.Presentation.dll
fi

# Iniciar aplicación
echo "Starting application..."
exec dotnet EventManagement.Presentation.dll