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

**Entities**: Configure relationships explicitly in `OnModelCreating`

- Navigation collections: Use **getter-only** properties `{ get; } = new List<T>()`
- Reference navigations: Standard properties with `= default!`

```csharp
// ✅ Collection navigation (one-to-many)
public ICollection<WeatherForecast> WeatherForecasts { get; } = new List<WeatherForecast>();

// ✅ Reference navigation (many-to-one)
public User User { get; set; } = default!;
```

**DbContext**:

- Call `base.OnModelCreating()` FIRST
- Configure all relationships explicitly
- Add indexes for foreign keys and frequently queried columns
- Uses snake_case via `UseSnakeCaseNamingConvention()`

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

### React Component Conventions

**Module Structure**: Each feature module follows this pattern:

```
login/
  ├── login.types.ts    # Types and validation schemas
  ├── login.ui.tsx      # Presentational components (dumb)
  └── login.view.tsx    # Business logic (smart component)
```

**Composition Pattern**: Use `Object.assign` to compose sub-components:

```tsx
// login.ui.tsx
function LoginFormRoot(props: LoginFormProps) {
  return (
    <Form {...props.form}>
      <form
        onSubmit={(e) => {
          void props.form.handleSubmit(props.onSubmit)(e);
        }}
      >
        {props.children}
      </form>
    </Form>
  );
}

function Email(props: EmailProps) {
  return (
    <FormItem>
      <FormLabel>Email</FormLabel>
      <FormControl>
        <Input type="email" {...props.field} />
      </FormControl>
      <FormMessage />
    </FormItem>
  );
}

function Password(props: PasswordProps) {
  /* ... */
}
function Submit(props: SubmitProps) {
  /* ... */
}

export const LoginForm = Object.assign(LoginFormRoot, {
  Email,
  Password,
  Submit,
});
```

**Usage in View**:

```tsx
// login.view.tsx
<LoginForm form={form} onSubmit={handleSubmit}>
  <FormField control={form.control} name="email" render={LoginForm.Email} />
  <FormField
    control={form.control}
    name="password"
    render={LoginForm.Password}
  />
  <LoginForm.Submit isPending={status === "pending"} />
</LoginForm>
```

**Component Conventions**:

1. **Named functions only** - No arrow functions for components

   ```tsx
   // ✅ Correct
   function Email(props: EmailProps) {}

   // ❌ Avoid
   const Email = (props: EmailProps) => {};
   ```

2. **Props non-destructured** - Always use `props.x`

   ```tsx
   // ✅ Correct
   function Email(props: EmailProps) {
     return <Input {...props.field} />;
   }

   // ❌ Avoid
   function Email({ field }: EmailProps) {
     return <Input {...field} />;
   }
   ```

3. **React.PropsWithChildren for children**

   ```tsx
   interface LoginFormProps extends React.PropsWithChildren {
     form: UseFormReturn<LoginFormSchema>;
     onSubmit: (values: LoginFormSchema) => void;
   }
   ```

4. **Short, direct names** - Composition sub-components use concise names

   ```tsx
   // ✅ Correct
   LoginForm.Email, LoginForm.Submit;

   // ❌ Avoid
   LoginForm.EmailField, LoginForm.SubmitButton;
   ```

5. **No barrel files (index.ts)** - Import directly from files

   ```tsx
   // ✅ Correct
   import { LoginForm } from "./login.ui";

   // ❌ Avoid
   import { LoginForm } from "./components";
   ```

6. **No folder for single file** - Use `.ui.tsx` at module root

   ```
   ✅ login/login.ui.tsx
   ❌ login/components/login-form.tsx (for single file)
   ```

7. **No comments** - Code should be self-documenting

8. **Form submission pattern**

   ```tsx
   <form onSubmit={(e) => { void form.handleSubmit(onSubmit)(e); }}>
   ```

9. **FormField shorthand** - When component only takes `field` prop

   ```tsx
   // ✅ Correct - component signature matches
   <FormField control={form.control} name="email" render={LoginForm.Email} />

   // ❌ Avoid - unnecessary wrapper
   <FormField control={form.control} name="email" render={({ field }) => <LoginForm.Email field={field} />} />
   ```

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
