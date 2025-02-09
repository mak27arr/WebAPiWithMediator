FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY Gateway/Gateway/Gateway.csproj Gateway/

RUN dotnet restore Gateway/Gateway.csproj

COPY Gateway/ .

RUN dotnet build Gateway/Gateway.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish Gateway/Gateway.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Gateway.dll"]
