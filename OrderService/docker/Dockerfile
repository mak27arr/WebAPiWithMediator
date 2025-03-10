FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY OrderService/OrderService.API/OrderService.API.csproj OrderService.API/
COPY OrderService/OrderService.Application/OrderService.Application.csproj OrderService.Application/
COPY OrderService/OrderService.Domain/OrderService.Domain.csproj OrderService.Domain/
COPY OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj OrderService.Infrastructure/
COPY NuGet.Config /root/.nuget/NuGet.Config

RUN dotnet restore OrderService.API/OrderService.API.csproj --configfile /root/.nuget/NuGet.Config

COPY OrderService/ .

RUN dotnet build OrderService.API/OrderService.API.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish OrderService.API/OrderService.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 5000

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OrderService.API.dll"]
