FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY Products/Products.API/Products.API.csproj Products.API/
COPY Products/Products.Core/Products.Core.csproj Products.Core/
COPY Products/Products.Infrastructure/Products.Infrastructure.csproj Products.Infrastructure/
COPY NuGet.Config /root/.nuget/NuGet.Config

RUN dotnet restore Products.API/Products.API.csproj --configfile /root/.nuget/NuGet.Config

COPY Products/ .

RUN dotnet build Products.API/Products.API.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish Products.API/Products.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 5000

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Products.API.dll"]
