FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY InventoryService/Inventory.Infrastructure/Inventory.Infrastructure.csproj Inventory.Infrastructure/
COPY InventoryService/Inventory.Domain/Inventory.Domain.csproj Inventory.Domain/
COPY InventoryService/Inventory.Application/Inventory.Application.csproj Inventory.Application/
COPY InventoryService/Inventory.API/Inventory.API.csproj Inventory.API/
COPY NuGet.Config /root/.nuget/NuGet.Config

RUN dotnet restore Inventory.API/Inventory.API.csproj --configfile /root/.nuget/NuGet.Config

COPY InventoryService/ .

RUN dotnet build Inventory.API/Inventory.API.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish Inventory.API/Inventory.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 5000

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Inventory.API.dll"]
