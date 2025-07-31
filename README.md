# React Template

## How to use this template?

1. Install the template

```bash
dotnet new install .
```

2. Create a new project (don't forget the --title)

```bash
dotnet new full-stack-react -o MyProject --title my-project
```

3. Remove this "how to use this template" section inside the README.md

## Getting started

1. Launch the database

```bash
docker compose up -d
```

2. Install deps

```bash
# From ./ReactTemplate.Server/
dotnet restore
```

```bash
# From ./ReactTemplate.Client/
npm install
```

2. Set environment variables

```bash
# From ./ReactTemplate.Server/
dotnet user-secrets init
dotnet user-secrets set "Smtp:Gmail:Username" "value"
dotnet user-secrets set "Smtp:Gmail:Password" "value"
dotnet user-secrets set "Database:ReactTemplate:ConnectionString" "value"
dotnet user-secrets set "OtlpKey" "value"
```

3. Launch the Debug configuration

## Deployment

```bash
# From the root (./)
rm -rf ReactTemplate.Server/bin/Publish && dotnet publish ReactTemplate.Server -t:PublishContainer -p ContainerArchiveOutputPath=../react-template.tar.gz -o ReactTemplate.Server/bin/Publish
```

## Migrations

From ReactTemplate.Server

```bash
# From ./ReactTemplate.Server/
dotnet ef migrations add MigrationName
```
