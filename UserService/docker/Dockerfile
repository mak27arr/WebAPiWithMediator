FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY UserService/UserService.Infrastructure/UserService.Infrastructure.csproj UserService.Infrastructure/
COPY UserService/UserService.Domain/UserService.Domain.csproj UserService.Domain/
COPY UserService/UserService.Application/UserService.Application.csproj UserService.Application/
COPY UserService/UserService.API/UserService.API.csproj UserService.API/
COPY NuGet.Config /root/.nuget/NuGet.Config

RUN dotnet restore UserService.API/UserService.API.csproj --configfile /root/.nuget/NuGet.Config

COPY UserService/ .

RUN dotnet build UserService.API/UserService.API.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish UserService.API/UserService.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 5000

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "UserService.API.dll"]
