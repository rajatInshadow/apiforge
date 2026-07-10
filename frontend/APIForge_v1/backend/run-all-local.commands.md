# Run All Backend Services

Open four terminals.

Terminal 1:

```bash
cd backend/src/APIForge.PortalApi
dotnet run --urls https://localhost:7101
```

Terminal 2:

```bash
cd backend/src/APIForge.Gateway
dotnet run --urls https://localhost:7201
```

Terminal 3:

```bash
cd backend/src/APIForge.Sample.ProductService
dotnet run --urls https://localhost:7301
```

Terminal 4:

```bash
cd backend/src/APIForge.Sample.OrderService
dotnet run --urls https://localhost:7401
```
