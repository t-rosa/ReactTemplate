# ReactTemplate.Tools - Scaffolding CLI

CLI dotnet global tool pour générer des entités complètes avec code boilerplate serveur et client.

## Installation

```bash
dotnet tool install --global --add-source ./ReactTemplate.Tools/nupkg ReactTemplate.Tools
```

Après installation, le commande `react-template-scaffold` sera disponible globalement.

## Utilisation

### Commande de base

```bash
react-template-scaffold scaffold --module Products --entity Product --fields "name:string required; price:decimal required; stock:int"
```

### Options disponibles

- `--module, -m` (obligatoire): Nom du module (ex: `Products`, `WeatherForecasts`)
- `--entity, -e` (obligatoire): Nom de l'entité (ex: `Product`, `WeatherForecast`)
- `--fields, -f` (optionnel): Définition des champs (ex: `name:string required; active:bool`)
- `--spec, -s` (optionnel): Chemin vers un fichier spec JSON/YAML pour les entités complexes
- `--with-tests` (défaut: true): Générer les fichiers de test (xUnit, Vitest, Playwright)
- `--skip-migration` (défaut: false): Ne pas suggérer les migrations EF
- `--dry-run` (défaut: false): Afficher un aperçu sans générer de fichiers

### Exemples

#### Génération simple avec champs inline

```bash
react-template-scaffold scaffold \
  --module Products \
  --entity Product \
  --fields "name:string required; description:string; price:decimal required; stock:int default=0; isActive:bool"
```

#### Génération depuis un fichier spec

```bash
react-template-scaffold scaffold \
  --spec ./specs/product-entity.json
```

#### Aperçu sans générer

```bash
react-template-scaffold scaffold \
  --module Products \
  --entity Product \
  --dry-run
```

#### Sans tests

```bash
react-template-scaffold scaffold \
  --module Products \
  --entity Product \
  --fields "name:string required" \
  --no-tests
```

## Fichiers Spec JSON

Pour les entités complexes, créez un fichier spec:

```json
{
  "entityName": "Product",
  "moduleName": "Products",
  "description": "Product entity for e-commerce",
  "hasAuditFields": true,
  "hasSoftDelete": false,
  "tags": ["e-commerce", "inventory"],
  "fields": [
    {
      "name": "Id",
      "type": "Guid",
      "isPrimaryKey": true
    },
    {
      "name": "Name",
      "type": "string",
      "required": true,
      "validation": "MinLength(3) && MaxLength(200)"
    },
    {
      "name": "Price",
      "type": "decimal",
      "required": true,
      "validation": "GreaterThan(0)"
    },
    {
      "name": "Stock",
      "type": "int",
      "required": true,
      "defaultValue": "0"
    }
  ]
}
```

## Fichiers Générés

### Serveur (.NET)

```
ReactTemplate.Server/Modules/{Module}/{Entity}/
├── {Entity}.cs                      # Entity class
├── {Entity}Controller.cs            # API Controller avec CRUD complet
├── Dtos/
│   ├── Create{Entity}Request.cs    # DTO pour création
│   ├── Update{Entity}Request.cs    # DTO pour mise à jour
│   └── Get{Entity}Response.cs      # DTO pour réponse
├── Validators/
│   └── {Entity}Validators.cs       # Validateurs FluentValidation
└── Tests/
    └── {Entity}ControllerTests.cs  # Tests xUnit avec Bogus + Testcontainers
```

### Client (React/TypeScript)

```
ReactTemplate.Client/src/modules/{module}/{entity}/
├── hooks.ts                        # Hooks TanStack Query (list, get, create, update, delete)
├── form.tsx                        # Formulaire React Hook Form + Zod
├── views/
│   ├── list.tsx                   # Vue liste (tableau)
│   ├── detail.tsx                 # Vue détail
│   └── create-edit.tsx            # Vue création/édition
├── {entity}.spec.ts               # Tests Vitest (form, hooks)
└── {entity}.e2e.spec.ts           # Tests Playwright (CRUD complet)
```

## Post-génération

Après la génération, le CLI suggère les prochaines étapes:

1. **Vérifier les fichiers générés** - Tous les fichiers sont générés dans le bon répertoire
2. **Ajouter les DTOs au DbContext** - Les entités générées doivent être enregistrées dans `ApplicationDbContext`
3. **Créer une migration EF**:
   ```bash
   dotnet ef migrations add Add{Entity}
   ```
4. **Appliquer la migration**:
   ```bash
   dotnet ef database update
   ```
5. **Formatter les fichiers**:
   - C#: `dotnet format`
   - TypeScript: `npm run lint -- --fix` ou `prettier --write`

## Types de champs supportés

### C# / TypeScript

| Type C#    | TypeScript       | Exemple                         |
| ---------- | ---------------- | ------------------------------- |
| `string`   | `string`         | `--fields "name:string"`        |
| `int`      | `number`         | `--fields "count:int"`          |
| `decimal`  | `number`         | `--fields "price:decimal"`      |
| `bool`     | `boolean`        | `--fields "isActive:bool"`      |
| `DateTime` | `Date \| string` | `--fields "createdAt:datetime"` |
| `Guid`     | `string`         | `--fields "id:guid"`            |

## Configuration par module

Les fichiers générés respectent les conventions du projet:

- **Module name**: Format PascalCase (ex: `Products`, `WeatherForecasts`)
- **Entity name**: Format PascalCase (ex: `Product`, `WeatherForecast`)
- **Routes API**: Format kebab-case (ex: `/api/products`, `/api/weather-forecasts`)
- **Client paths**: Format kebab-case (ex: `/products`, `/weather-forecasts`)
- **DbSet**: Format camelCase (ex: `products`, `weatherForecasts`)

## Architecture générée

### Serveur

- **Entity**: Hérite de `AuditableEntity` (CreatedAt, UpdatedAt automatiques)
- **Controller**: Endpoints CRUD avec `[Authorize]` par défaut
- **DTOs**: Séparation des concerns (Create, Update, Get)
- **Validators**: FluentValidation intégré
- **Tests**: xUnit + Bogus pour données fake + Testcontainers pour intégration

### Client

- **Hooks**: TanStack Query avec invalidation automatique
- **Forms**: React Hook Form + Zod (validation client + serveur)
- **Views**: Composants réutilisables (List, Detail, Form)
- **Tests**: Vitest (unitaire) + Playwright (e2e)

## Limites actuelles

- Les relations 1-N sont générées avec clé étrangère `UserId` (multi-ténant par utilisateur)
- Les relations N-N ne sont pas (encore) supportées
- Les enums doivent être créés manuellement
- Les propriétés calculées ne sont pas supportées

## Troubleshooting

### "Could not find ReactTemplate.slnx"

Assurez-vous que vous exécutez la commande depuis le répertoire racine du projet (où se trouve `ReactTemplate.slnx`).

```bash
cd /path/to/ReactTemplate
react-template-scaffold scaffold --module Products --entity Product
```

### "Required directory not found"

La structure du projet ne respecte pas les conventions. Vérifiez:

- `ReactTemplate.Server/Modules/` existe
- `ReactTemplate.Client/src/modules/` existe

### Migrations en conflit

Si vous avez des conflits de migration après génération:

```bash
# Optionnel: Supprimer la dernière migration non appliquée
dotnet ef migrations remove

# Créer une nouvelle migration
dotnet ef migrations add Add{Entity}

# Appliquer
dotnet ef database update
```

## Développement

Pour construire et tester localement:

```bash
cd ReactTemplate.Tools
dotnet build
dotnet pack
dotnet tool install --global --add-source ./bin/Debug/package ReactTemplate.Tools --force
```

Puis tester:

```bash
react-template-scaffold scaffold --help
```

## Licences

MIT - Voir LICENSE.md
