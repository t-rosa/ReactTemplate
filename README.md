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
# SMTP (Gmail)
dotnet user-secrets init
dotnet user-secrets set "Smtp:Username" "value"
dotnet user-secrets set "Smtp:Password" "value"
dotnet user-secrets set "ReactTemplate:ConnectionString" "value"
```

3. Setup the database

```bash
# From ./ReactTemplate.Server/
dotnet ef database update
```

4. Launch the Debug configuration

## Deployment

```bash
rm -rf ReactTemplate.Server/bin/Publish && dotnet publish ReactTemplate.Server -t:PublishContainer -p ContainerArchiveOutputPath=../react-template.tar.gz -o ReactTemplate.Server/bin/Publish
```
