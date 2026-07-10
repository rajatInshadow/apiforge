# Solution Setup Instructions

Run the following from this `backend/` folder on your machine:

```bash
dotnet new sln -n APIForge

dotnet sln add src/APIForge.Shared/APIForge.Shared.csproj
dotnet sln add src/APIForge.Infrastructure/APIForge.Infrastructure.csproj
dotnet sln add src/APIForge.PortalApi/APIForge.PortalApi.csproj
dotnet sln add src/APIForge.Gateway/APIForge.Gateway.csproj
dotnet sln add src/APIForge.Sample.ProductService/APIForge.Sample.ProductService.csproj
dotnet sln add src/APIForge.Sample.OrderService/APIForge.Sample.OrderService.csproj
```

Then restore and build:

```bash
dotnet restore
dotnet build
```
