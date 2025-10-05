# ReactTemplate Copilot Instructions

## Architecture Overview

Full-stack .NET + React template with modular vertical slice architecture:

- **Server**: ASP.NET Core 9.0 minimal API with EF Core, Identity, OpenAPI
- **Client**: React + TypeScript with TanStack Router/Query, Vite, Tailwind + shadcn/ui
- **Communication**: Type-safe API via `openapi-typescript` auto-generated schemas

## Module Organization (Vertical Slices)

Both client and server follow **feature-based modules**, not layered architecture:

### Server (`ReactTemplate.Server/Modules/`)

Each module contains its complete feature: Controller, DTOs, Validators, Entity

```
WeatherForecasts/
  ├── WeatherForecast.cs          # Entity
  ├── WeatherForecastsController.cs
  └── Dtos/
      ├── CreateWeatherForecastRequest.cs  # with FluentValidation validator class
      └── GetWeatherForecastResponse.cs
```

### Client (`ReactTemplate.Client/src/modules/`)

Each module contains views, components, and business logic:

```
app/forecasts/
  ├── forecasts.view.tsx          # Main view component
  ├── create-forecast/
  │   └── create-forecast.view.tsx
  └── forecast-actions/
      └── forecast-actions.view.tsx
```

## Critical Workflows

### API Contract Generation

**Must run after backend changes:**

```bash
cd ReactTemplate.Client
npm run openapi  # Generates src/lib/api/schema.ts from Server OpenAPI spec
```

This generates TypeScript types from `ReactTemplate.Server/obj/ReactTemplate.Server.json`.

### Database Migrations

```bash
cd ReactTemplate.Server
dotnet ef migrations add MigrationName
```

Migrations auto-run in Development via `Program.cs: context.Database.Migrate()`.

### Build & Run

```bash
# Development (separate processes)
docker compose up -d              # Starts PostgreSQL + telemetry
dotnet run --project ReactTemplate.Server  # Server on https://localhost:7000
cd ReactTemplate.Client && npm run dev      # Client proxies to server

# Production build
dotnet publish ReactTemplate.Server -o ReactTemplate.Server/bin/Production
# Builds client assets into server's wwwroot
```

### Testing

```bash
# Server: XUnit integration tests with Testcontainers
dotnet test ReactTemplate.Tests

# Client: Playwright E2E
cd ReactTemplate.Client && npm run test:e2e
```

## Key Patterns

### Server Conventions

**Controllers**: Always in module root, inherit `ControllerBase`, use `[Authorize]` + `[AllowAnonymous]`

```csharp
[ApiController]
[Route("api/weather-forecasts")]
[Authorize]
public class WeatherForecastsController : ControllerBase
```

**DTOs**: Use records with inline FluentValidation validators

```csharp
public record CreateWeatherForecastRequest(int TemperatureC, string? Summary);

public class CreateWeatherForecastRequestValidator : AbstractValidator<CreateWeatherForecastRequest>
```

**DbContext**: Entities added as `DbSet<T>` properties in `ApplicationDbContext.cs`, uses snake_case via `UseSnakeCaseNamingConvention()`.

**Seeding**: `SeedData.cs` runs on startup, creates roles (Admin/Member/User) and admin user from secrets.

### Client Conventions

**API Calls**: Use `$api` from `@/lib/api/client.ts` - wraps openapi-fetch with TanStack Query

```tsx
const { data } = $api.useSuspenseQuery("get", "/api/weather-forecasts");
const { mutate } = $api.useMutation("post", "/api/weather-forecasts", {
  onSuccess: () =>
    queryClient.invalidateQueries({
      queryKey: ["get", "/api/weather-forecasts"],
    }),
});
```

**Routing**: File-based via `@tanstack/router` - routes in `src/routes/`, auto-generates `routeTree.gen.ts`

```tsx
// src/routes/_app/forecasts/index.tsx
export const Route = createFileRoute("/_app/forecasts/")({
  loader({ context }) {
    return context.queryClient.ensureQueryData(
      $api.queryOptions("get", "/api/weather-forecasts")
    );
  },
  component: ForecastsView,
});
```

**Components**: shadcn/ui in `src/components/ui/`, use CVA for variants. Import via `@/` alias.

**Types**: Reference OpenAPI schemas: `components["schemas"]["GetWeatherForecastResponse"]`

## Configuration

### Server Secrets (Development)

```bash
cd ReactTemplate.Server
dotnet user-secrets set "ADMIN_EMAIL" "admin@example.com"
dotnet user-secrets set "ADMIN_PASSWORD" "SecurePass123!"
dotnet user-secrets set "CONNECTION_STRING" "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=react-template;"
dotnet user-secrets set "SMTP_USERNAME" "your-smtp-user"
dotnet user-secrets set "SMTP_PASSWORD" "your-smtp-pass"
```

### Docker Services

- **PostgreSQL**: `localhost:5432` (react-template-db)
- **Aspire Dashboard**: `localhost:18888` (OpenTelemetry UI)

## Project Structure Notes

- **Modules/Auth**: Identity endpoints (register/login/logout/forgotPassword) - custom controller wrapping ASP.NET Identity
- **Modules/Users**: User management, extends `IdentityUser<Guid>`
- **Integration Tests**: Use `BaseFactory` with Testcontainers PostgreSQL on port 5555
- **Client routing**: `_app` = authenticated, `_auth` = auth pages, `_marketing` = public
- **Vite proxy**: Dev client proxies `/api` to server (see `vite.config.ts`)

## Common Tasks

**Add new feature:**

1. Create module folder in both Server/Modules and Client/src/modules
2. Server: Add controller, DTOs with validators, register in DbContext if needed
3. Client: Create route file, view component, API calls via `$api`
4. Run `npm run openapi` to sync types

**Update UI component:**
Use shadcn/ui CLI: `npx shadcn@latest add [component]` (configured via `components.json`)

**Debug full stack:**
See README for VSCode compound launch config (Server + Client + Browser).
