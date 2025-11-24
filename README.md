# API Versioning

A comprehensive .NET 10 API versioning solution demonstrating modern best practices for API versioning using ASP.NET Core.

## Overview

This solution showcases how to implement API versioning in ASP.NET Core with support for:
- **URL segment versioning** (`/api/v1/assets`, `/api/v2/assets`)
- **Header-based versioning** (via `x-api-version` header)
- **Version deprecation** with automatic documentation
- **Multiple API versions** running side-by-side
- **OpenAPI documentation** per version using Scalar

## Architecture

The solution consists of two projects:

### ApiVersioning.WebApi
The main Web API project containing:
- Controllers with version-specific implementations
- API versioning configuration
- OpenAPI/Swagger documentation setup
- Scalar UI for interactive API documentation

### ApiVersioning.Dtos
Shared DTOs organized by version:
- `V1.Models` - Version 1 models
- `V2.Models` - Version 2 models
- `Domain.cs` - Shared domain types

## Features

### 1. Multiple Versioning Strategies

The API supports two versioning methods simultaneously:

**URL Segment (Primary)**
```
GET /api/v1/assets/{id}
GET /api/v2/assets/{id}
```

**Header-Based (Fallback)**
```
GET /api/assets/{id}
Header: x-api-version: 2
```

### 2. Version Deprecation

Version 1 is marked as deprecated:
- `[ApiVersion("1", Deprecated = true)]` attribute on controller
- Automatic deprecation notices in OpenAPI documentation
- Response headers indicate supported and deprecated versions

### 3. Default Version Handling

- Default API version: **v2.0**
- Requests without version specification automatically use v2
- `AssumeDefaultVersionWhenUnspecified = true` enables backward compatibility

### 4. API Version Reporting

The API includes version information in response headers:
- `api-supported-versions` - Lists all available versions
- `api-deprecated-versions` - Lists deprecated versions

## Implementation Details

### Program.cs Configuration

```csharp
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version")
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
```

### Controller Versioning

**Version 1 (Deprecated)**
```csharp
[ApiVersion("1", Deprecated = true)]
[ApiController]
[Route("api/v{v:apiVersion}/assets")]
public class AssetsV1Controller : ControllerBase
{
    // V1 implementation
}
```

**Version 2 (Current)**
```csharp
[ApiVersion("2")]
[ApiController]
[Route("api/v{v:apiVersion}/assets")]
[Route("api/assets")]  // Default route for unversioned requests
public class AssetsV2Controller : ControllerBase
{
    // V2 implementation
}
```

### Model Evolution

**V1 Model Changes:**
- `Id` property name
- `Value` for asset value

**V2 Model Improvements:**
- `AssetId` (renamed from `Id` for clarity)
- `TotalValue` (renamed from `Value`)
- `RetiredOn` (new property for retirement tracking)

## API Endpoints

### Assets V1 (Deprecated)

#### Update Asset
```http
PATCH /api/v1/assets?id={assetId}
Content-Type: application/json

{
  "status": "Active",
  "notes": "Updated notes",
  "value": 1500.00
}
```

#### Get Asset
```http
GET /api/v1/assets/{id}
```

### Assets V2 (Current)

#### Update Asset
```http
PATCH /api/v2/assets?id={assetId}
Content-Type: application/json

{
  "status": "Active",
  "notes": "Updated notes",
  "totalValue": 1500.00,
  "retiredOn": "2024-12-31T00:00:00Z"
}
```

#### Get Asset
```http
GET /api/v2/assets/{id}
```

## Running the Application

### Prerequisites
- .NET 10 SDK
- Visual Studio 2025 or later (or VS Code)

### Build and Run
```bash
cd src/ApiVersioning.WebApi
dotnet build
dotnet run
```

### Access API Documentation
Once running, navigate to:
```
https://localhost:{port}/scalar/v1  # V1 API Documentation
https://localhost:{port}/scalar/v2  # V2 API Documentation
```

The Scalar UI provides:
- Interactive API documentation
- Request/response examples
- Version-specific endpoint information
- Deprecation warnings for V1

## NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Asp.Versioning.Mvc | 8.1.0 | Core API versioning functionality |
| Asp.Versioning.Mvc.ApiExplorer | 8.1.0 | OpenAPI integration for versioned APIs |
| Microsoft.AspNetCore.OpenApi | 10.0.0 | OpenAPI document generation |
| Microsoft.OpenApi | 2.0.0 | OpenAPI specification support |
| Scalar.AspNetCore | 2.11.0 | Modern API documentation UI |

## Best Practices Demonstrated

1. **Semantic Versioning**: Major version in URL for breaking changes
2. **Backward Compatibility**: Default version ensures existing clients continue working
3. **Clear Deprecation Strategy**: Explicit marking of deprecated versions
4. **Namespace Organization**: Version-specific DTOs in separate namespaces
5. **Documentation**: Auto-generated docs with version-specific details
6. **Multiple Reader Support**: Flexibility in how clients specify versions
7. **Controller Separation**: Separate controllers per version for maintainability

## Migration Guide (V1 → V2)

When migrating from V1 to V2, update your client code:

### Property Name Changes
```csharp
// V1
response.Id          → response.AssetId  // V2
response.Value       → response.TotalValue  // V2
```

### New Properties
```csharp
// V2 adds:
response.RetiredOn   // Optional DateTime for retirement tracking
```

### URL Updates
```csharp
// Both versions support:
/api/v1/assets/{id}  // V1 (deprecated)
/api/v2/assets/{id}  // V2 (current)
/api/assets/{id}     // Defaults to V2
```

## Future Considerations

- **Version Sunset**: Plan to remove V1 after migration period
- **API Analytics**: Track version usage to inform deprecation timeline
- **Client Communication**: Notify clients of deprecation via headers and documentation
- **Testing**: Maintain integration tests for all supported versions

## Contributing

When adding new API versions:
1. Create new namespace in `ApiVersioning.Dtos` (e.g., `V3.Models`)
2. Add new controller with `[ApiVersion("3")]` attribute
3. Update `Program.cs` to include new version in OpenAPI configuration
4. Update this README with migration guide

## License

This project is provided as-is for educational and demonstration purposes.
