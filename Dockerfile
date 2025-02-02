# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the entire project to the container
COPY . . 

# Restore dependencies and publish the project
RUN dotnet restore WebAPI.API/WebAPI.API.csproj
RUN dotnet publish WebAPI.API/WebAPI.API.csproj -c Release -o out --no-restore

# Final Stage (Runtime Image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy build artifacts from the build stage
COPY --from=build /app/out ./

# Install PostgreSQL client for health checks and waiting for DB to be ready
RUN apt-get update && apt-get install -y postgresql-client

# Copy docker-entrypoint.sh to the final image and make it executable
COPY docker-entrypoint.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/docker-entrypoint.sh
RUN ls -lah /usr/local/bin/

# Expose port for your app
EXPOSE 5000

# Set the entrypoint for the application
ENTRYPOINT ["/bin/bash", "-c", "/usr/local/bin/docker-entrypoint.sh"]