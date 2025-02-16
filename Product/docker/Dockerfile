FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY Product/WebAPI.ProductAPI/WebAPI.ProductAPI.csproj WebAPI.ProductAPI/
COPY Product/WebAPI.Core/WebAPI.Core.csproj WebAPI.Core/
COPY Product/WebAPI.Infrastructure/WebAPI.Infrastructure.csproj WebAPI.Infrastructure/
COPY NuGet.Config /root/.nuget/NuGet.Config

RUN dotnet restore WebAPI.ProductAPI/WebAPI.ProductAPI.csproj --configfile /root/.nuget/NuGet.Config

COPY Product/ .

RUN dotnet build WebAPI.ProductAPI/WebAPI.ProductAPI.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish WebAPI.ProductAPI/WebAPI.ProductAPI.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 5000

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebAPI.ProductAPI.dll"]
