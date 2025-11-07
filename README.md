# React Template

Full-stack starter that ships a production-ready ASP.NET Core API with a modern React front-end, wired together with Identity, EF Core, and a PostgreSQL dev database.

## Template Quickstart _(delete after scaffolding)_

1. Install the template

```bash
dotnet new install .
```

2. Scaffold a new project

```bash
dotnet new full-stack-react -o MyProject
```

Every occurrence of `ReactTemplate`/`react-template` is replaced automatically. `MyProject` becomes `my-project`, so the generated README and configuration already point to the correct projects and secrets. Remove this section once you copy the template into a new repository.

## Prerequisites

- [Git](https://git-scm.com/)
- [Docker](https://www.docker.com/)
- [Node.js](https://nodejs.org)
- [.NET](https://dotnet.microsoft.com/en-us/download)
  - Tools .NET
    - [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

### Recommended

- [GitHub CLI](https://cli.github.com/)
- [pgAdmin](https://www.pgadmin.org/)

## Getting started

1. Initialize git

```bash
git init
```

2. Launch the database

```bash
docker compose up -d
```

3. Configure required secrets

```bash
dotnet user-secrets --project ReactTemplate.Server set "SMTP_USERNAME" "value"
```

```bash
dotnet user-secrets --project ReactTemplate.Server set "SMTP_PASSWORD" "value"
```

```bash
dotnet user-secrets --project ReactTemplate.Server set "ADMIN_EMAIL" "value"
```

```bash
dotnet user-secrets --project ReactTemplate.Server set "ADMIN_PASSWORD" "value"
```

```bash
dotnet user-secrets --project ReactTemplate.Server set "CONNECTION_STRING" "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=react-template;"
```

4. Build the app

```bash
dotnet publish ReactTemplate.Server -o ReactTemplate.Server/bin/Production
```

5. Launch the app

```bash
dotnet run --project ReactTemplate.Server
```

- Execute automated tests:

```bash
dotnet test ReactTemplate.Tests
```

Run the server end-to-end commands from the root : `ReactTemplate` directory.

```bash
cd ReactTemplate.Client
npm run test
npx playwright test
```

Run the client and end-to-end commands from the `ReactTemplate.Client` directory.

## Debug

- VSCode

```json
// .vscode/launch.json
{
  "configurations": [
    {
      "name": "Server",
      "type": "dotnet",
      "request": "launch",
      "projectPath": "${workspaceFolder}/ReactTemplate.Server/ReactTemplate.Server.csproj"
    },
    {
      "name": "Client",
      "runtimeArgs": ["run-script", "dev"],
      "runtimeExecutable": "npm",
      "skipFiles": ["<node_internals>/**"],
      "type": "node",
      "request": "launch",
      "cwd": "${workspaceFolder}/ReactTemplate.Client"
    },
    {
      "name": "Browser",
      "request": "launch",
      "type": "msedge",
      "url": "https://localhost:7000",
      "webRoot": "${workspaceFolder}/ReactTemplate.Client"
    }
  ],
  "compounds": [
    {
      "name": "Debug",
      "configurations": ["Server", "Client", "Browser"],
      "presentation": {
        "hidden": false,
        "group": "",
        "order": 1
      },
      "stopAll": true
    }
  ]
}
```

## Production Preview

1. Publish the container image

```bash
rm -rf ReactTemplate.Server/bin/Production/ && dotnet publish ReactTemplate.Server -t:PublishContainer -p ContainerArchiveOutputPath=../server.tar.gz -o ReactTemplate.Server/bin/Production
```

2. Load the Docker image

```bash
docker load < server.tar.gz
```

3. Stop any running containers

```bash
docker compose down
```

4. Enable the `preview` service inside `compose.yaml`.

5. Restart the stack

```bash
docker compose up -d
```

6. Browse to `http://localhost:3000`.

## Deployment

```bash
rm -rf ReactTemplate.Server/bin/Publish && dotnet publish ReactTemplate.Server -t:PublishContainer -p ContainerArchiveOutputPath=../react-template.tar.gz -o ReactTemplate.Server/bin/Publish
```

## Migrations

From ReactTemplate.Server

```bash
dotnet ef migrations add MigrationName --project ReactTemplate.Server
```

## Technologies

### Server

- [ASP NET Core](https://dotnet.microsoft.com/apps/aspnet)
- [EFCore](https://learn.microsoft.com/en-us/ef/core/)
- [Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [FluentValidation](https://fluentvalidation.net/)
- [XUnit](https://xunit.net/)
- [Testcontainers](https://testcontainers.com/)
- [FluentAssertion](https://fluentassertions.com/)
- [Bogus](https://github.com/bchavez/Bogus)

---

### Client

- [Vite](https://vitejs.dev/)
- [React](https://react.dev/)
- [TypeScript](https://www.typescriptlang.org/)
- [Tanstack/Router](https://tanstack.com/router/)
- [Tanstack/Query](https://tanstack.com/query/)
- [Tanstack/Table](https://tanstack.com/table/)
- [ReactHookForm](https://react-hook-form.com/)
- [openapi-typescript](https://openapi-ts.dev/)
- [openapi-fetch](https://openapi-ts.dev/openapi-fetch/)
- [openapi-react-query](https://openapi-ts.dev/openapi-react-query/)
- [Lucide](https://lucide.dev/)
- [Tailwind](https://tailwindcss.com/)
- [Shadcn/ui](https://ui.shadcn.com/)
- [Zod](https://zod.dev/)
- [ESLint](https://eslint.org/)
- [Prettier](https://prettier.io/)
- [Vitest](https://vitest.dev/)
- [Playwright](https://playwright.dev/)
