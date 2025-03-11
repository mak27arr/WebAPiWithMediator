FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY Auth/JwtAuthAPI/JwtAuthAPI.csproj JwtAuthAPI/
COPY Auth/JwtAuthManager/JwtAuthManager.csproj JwtAuthManager/
COPY NuGet.Config /root/.nuget/NuGet.Config

RUN dotnet restore JwtAuthAPI/JwtAuthAPI.csproj --configfile /root/.nuget/NuGet.Config

COPY Auth/ .

RUN dotnet build JwtAuthAPI/JwtAuthAPI.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish JwtAuthAPI/JwtAuthAPI.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 5000

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "JwtAuthAPI.dll"]
