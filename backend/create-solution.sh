#!/usr/bin/env bash
set -e
dotnet new sln -n APIForge

dotnet sln add src/APIForge.Shared/APIForge.Shared.csproj
dotnet sln add src/APIForge.Infrastructure/APIForge.Infrastructure.csproj
dotnet sln add src/APIForge.PortalApi/APIForge.PortalApi.csproj
dotnet sln add src/APIForge.Gateway/APIForge.Gateway.csproj
dotnet sln add src/APIForge.Sample.ProductService/APIForge.Sample.ProductService.csproj
dotnet sln add src/APIForge.Sample.OrderService/APIForge.Sample.OrderService.csproj

dotnet restore
dotnet build
