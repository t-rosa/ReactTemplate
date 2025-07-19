# React Template

## Template installation

```bash
dotnet new install .
```

## Create a new project

```bash
dotnet new full-stack-react -o MyProject --title my-project
```

## Deployment

```bash
rm -rf ReactTemplate.Server/bin/Publish && dotnet publish ReactTemplate.Server -t:PublishContainer -p ContainerArchiveOutputPath=../react-template.tar.gz -o ReactTemplate.Server/bin/Publish
```
