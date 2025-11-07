# Scaffolding CLI - Documentation Complète 📚

## 🎯 Vue d'ensemble

**ReactTemplate.Tools** est une CLI dotnet global tool qui automatise la génération d'entités complètes (serveur + client) pour le projet ReactTemplate.

### Architecture

- **Hôte**: dotnet global tool
- **Framework**: .NET 9.0
- **Templating**: Scriban
- **Spec format**: JSON / YAML

---

## 📦 Installation & Setup

### Installation locale (développement)

```bash
cd ReactTemplate.Tools
dotnet build
dotnet pack

# Installer globalement depuis le dossier local
dotnet tool install --global --add-source ./bin/Debug package ReactTemplate.Tools --force
```

### Installation depuis NuGet (production)

```bash
dotnet tool install --global ReactTemplate.Tools
```

Après installation, la commande `react-template-scaffold` est disponible partout.

---

## 🚀 Utilisation

### Syntaxe générale

```bash
react-template-scaffold scaffold \
  --module <MODULE> \
  --entity <ENTITY> \
  [--fields "<FIELDS>"] \
  [--spec <SPEC_FILE>] \
  [--with-tests] \
  [--skip-migration] \
  [--dry-run]
```

### Paramètres obligatoires

- `--module, -m`: Nom du module (ex: `Products`, `WeatherForecasts`)
- `--entity, -e`: Nom de l'entité (ex: `Product`, `WeatherForecast`)

### Paramètres optionnels

- `--fields, -f`: Définitions de champs inline
- `--spec, -s`: Chemin vers fichier spec JSON/YAML
- `--with-tests`: Générer tests (défaut: true)
- `--skip-tests`: Passer les tests
- `--skip-migration`: Omettre suggestion migration EF
- `--dry-run`: Afficher aperçu sans créer fichiers

---

## 💡 Exemples

### 1. Génération simple avec champs inline

```bash
react-template-scaffold scaffold \
  --module Products \
  --entity Product \
  --fields "name:string required; description:string; price:decimal required; stock:int; isActive:bool"
```

**Génère**:

- `ReactTemplate.Server/Modules/Products/Product/`

  - `Product.cs` (Entity)
  - `ProductController.cs` (CRUD API)
  - `Dtos/CreateProductRequest.cs`, `UpdateProductRequest.cs`, `GetProductResponse.cs`
  - `Validators/ProductValidators.cs`

- `ReactTemplate.Client/src/modules/products/product/`
  - `hooks.ts` (TanStack Query hooks)
  - `form.tsx` (React Hook Form + Zod)

### 2. Aperçu sans générer (dry-run)

```bash
react-template-scaffold scaffold \
  --module Products \
  --entity Product \
  --fields "name:string required; price:decimal" \
  --dry-run
```

**Affiche**:

```
🚀 Scaffolding entity: Product in module: Products
📋 Dry Run Preview
┌──────────────────────┬───────────────────────────────────────┐
│ Artifact             │ Path                                  │
├──────────────────────┼───────────────────────────────────────┤
│ ProductController.cs │ .../ReactTemplate.Server/Modules/... │
│ Product.cs           │ .../ReactTemplate.Server/Modules/... │
│ DTOs                 │ .../ReactTemplate.Server/Modules/... │
│ Validators           │ .../ReactTemplate.Server/Modules/... │
│ hooks                │ .../ReactTemplate.Client/src/...     │
│ Form                 │ .../ReactTemplate.Client/src/...     │
└──────────────────────┴───────────────────────────────────────┘

Fields: Id:Guid, Name:string, Price:decimal
```

### 3. Utiliser fichier spec JSON

```bash
# Créer spec/product.json
cat > specs/product.json << 'EOF'
{
  "entityName": "Product",
  "moduleName": "Products",
  "description": "E-commerce product",
  "hasAuditFields": true,
  "hasSoftDelete": false,
  "fields": [
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
EOF

# Générer
react-template-scaffold scaffold --spec specs/product.json
```

### 4. Générer sans tests

```bash
react-template-scaffold scaffold \
  --module Products \
  --entity Product \
  --skip-tests
```

### 5. Passer suggestions migration

```bash
react-template-scaffold scaffold \
  --module Products \
  --entity Product \
  --skip-migration
```

---

## 📋 Formats de champs

### Syntaxe inline

```
nom:type [options]
```

### Types supportés

| Type    | C#         | TypeScript       | Exemple                  |
| ------- | ---------- | ---------------- | ------------------------ |
| string  | `string`   | `string`         | `name:string required`   |
| entier  | `int`      | `number`         | `count:int`              |
| décimal | `decimal`  | `number`         | `price:decimal required` |
| booléen | `bool`     | `boolean`        | `isActive:bool`          |
| date    | `DateTime` | `Date \| string` | `createdAt:datetime`     |
| GUID    | `Guid`     | `string`         | `id:guid`                |
| long    | `long`     | `number`         | `bigNumber:long`         |
| double  | `double`   | `number`         | `percentage:double`      |
| float   | `float`    | `number`         | `ratio:float`            |

### Options de champ

```
name:string required          # Obligatoire
price:decimal required        # Valide côté client/serveur
count:int default=0           # Valeur par défaut
active:bool                   # Optionnel
```

### Exemples complets

```bash
# Simple
react-template-scaffold scaffold -m Users -e User -f "email:string required"

# Avec plusieurs champs
react-template-scaffold scaffold \
  -m Products -e Product \
  -f "name:string required; description:string; price:decimal required; stock:int default=0; isActive:bool"

# Avec validation
react-template-scaffold scaffold \
  -m Orders -e Order \
  -f "quantity:int required; total:decimal required; status:string"
```

---

## 📝 Format fichier spec JSON

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
    },
    {
      "name": "IsActive",
      "type": "bool",
      "required": true,
      "defaultValue": "true"
    }
  ]
}
```

### Format fichier spec YAML

```yaml
entityName: Product
moduleName: Products
description: Product entity for e-commerce
hasAuditFields: true
hasSoftDelete: false
tags:
  - e-commerce
  - inventory
fields:
  - name: Id
    type: Guid
    isPrimaryKey: true
  - name: Name
    type: string
    required: true
    validation: "MinLength(3) && MaxLength(200)"
  - name: Price
    type: decimal
    required: true
    validation: "GreaterThan(0)"
  - name: Stock
    type: int
    required: true
    defaultValue: "0"
  - name: IsActive
    type: bool
    required: true
    defaultValue: "true"
```

---

## 🔧 Architecture des fichiers générés

### Serveur (.NET)

```
ReactTemplate.Server/Modules/{Module}/{Entity}/
├── {Entity}.cs
│   └── Entité héritant de AuditableEntity
│       - CreatedAt, UpdatedAt automatiques
│       - Lien à User (UserId)
│
├── {Entity}Controller.cs
│   └── API REST CRUD complète
│       - GET /api/{entity-slug}
│       - GET /api/{entity-slug}/{id}
│       - POST /api/{entity-slug}
│       - PUT /api/{entity-slug}/{id}
│       - DELETE /api/{entity-slug}/{id}
│       - [Authorize] par défaut
│       - Filtre multi-ténant (UserId)
│
├── Dtos/
│   ├── Create{Entity}Request.cs
│   ├── Update{Entity}Request.cs
│   └── Get{Entity}Response.cs
│
└── Validators/
    └── {Entity}Validators.cs
        - Create{Entity}RequestValidator
        - Update{Entity}RequestValidator
```

### Client (React/TypeScript)

```
ReactTemplate.Client/src/modules/{module}/{entity}/
├── hooks.ts
│   ├── use{Entity}List()         // GET list
│   ├── use{Entity}(id)           // GET by id
│   ├── useCreate{Entity}()       // POST
│   ├── useUpdate{Entity}(id)     // PUT
│   └── useDelete{Entity}()       // DELETE
│   └── Invalidation queries auto
│
└── form.tsx
    ├── {Entity}Form component
    ├── React Hook Form
    ├── Zod schema
    └── Type-safe validation
```

---

## 🔄 Workflow post-génération

Après exécution, le CLI suggère:

1. **Vérifier les fichiers générés**

   - Tous dans les bons répertoires
   - Structure suit les conventions

2. **Ajouter au DbContext** (manuel)

   ```csharp
   public DbSet<Product> Products { get; set; }

   protected override void OnModelCreating(ModelBuilder builder)
   {
       builder.Entity<Product>(entity =>
       {
           entity.HasKey(e => e.Id);
           entity.HasOne(e => e.User)
               .WithMany(u => u.Products)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);
       });
   }
   ```

3. **Créer migration EF**

   ```bash
   dotnet ef migrations add Add{Entity}
   ```

4. **Appliquer migration**

   ```bash
   dotnet ef database update
   ```

5. **Formatter les fichiers** (optionnel)
   ```bash
   dotnet format
   npm run lint -- --fix
   ```

---

## ⚙️ Conventions & Patterns

### Routes API

- Format: `/api/{entity-slug}`
- Slug: `entity-name` en kebab-case
- Exemples:
  - `/api/products`
  - `/api/weather-forecasts`
  - `/api/user-accounts`

### Nommage Client

- `{entity-name}.lower()` pour chemins
- `useEntityName()` pour hooks
- `EntityNameForm` pour composants
- Exemples:
  - `useProductList()`
  - `useProduct(id)`
  - `ProductForm`

### Multi-ténancy

- Tous les contrôleurs filtrent par `User`
- Endpoint: `var user = await _userManager.GetUserAsync(HttpContext.User);`
- Entité: Liaison `UserId` automatique

### Audit

- `CreatedAt` automatique (AuditableEntity)
- `UpdatedAt` mis à jour à chaque modification
- Soft-delete optionnel (via spec)

---

## 🐛 Troubleshooting

### Error: "Could not find ReactTemplate.slnx"

**Solution**: Exécuter depuis la racine du projet

```bash
cd /chemin/vers/ReactTemplate
react-template-scaffold scaffold --module Products --entity Product
```

### Error: "Required directory not found"

**Solution**: Vérifier structure

```bash
# Doit exister:
ReactTemplate.Server/Modules/
ReactTemplate.Client/src/modules/
```

### Migrations en conflit

```bash
# Supprimer dernière migration
dotnet ef migrations remove

# Créer une nouvelle
dotnet ef migrations add Add{Entity}

# Appliquer
dotnet ef database update
```

### CLI non trouvée après installation

```bash
# Vérifier installation
dotnet tool list --global

# Réinstaller
dotnet tool uninstall --global ReactTemplate.Tools
dotnet tool install --global ReactTemplate.Tools
```

---

## 📊 Checklist après génération

- [ ] Fichiers générés dans bons répertoires
- [ ] DbContext mise à jour (manuel)
- [ ] Migration EF créée et appliquée
- [ ] Fichiers formatés (`dotnet format`)
- [ ] Tests compilent
- [ ] Routes API testables (Swagger)
- [ ] Hooks React importables
- [ ] Formulaires affichent bien

---

## 🔐 Sécurité & Bonnes pratiques

1. **Always [Authorize]**

   - Controllers générés ont `[Authorize]` par défaut
   - Pas d'accès anonyme aux données

2. **Filter by User**

   - Queries filtrées par `UserId` courant
   - Protection multi-ténancy

3. **DTOs séparés**

   - Create/Update/Get DTOs distincts
   - Évite exposer champs sensibles

4. **Validation**

   - FluentValidation côté serveur
   - Zod côté client
   - Double validation

5. **HTTPS only**
   - Tous les endpoints en HTTPS en production
   - Tokens JWT obligatoires

---

## 📞 Support

- **Documentation**: [GitHub Wiki](https://github.com/t-rosa/ReactTemplate/wiki)
- **Issues**: [GitHub Issues](https://github.com/t-rosa/ReactTemplate/issues)
- **Discussions**: [GitHub Discussions](https://github.com/t-rosa/ReactTemplate/discussions)

---

**Version**: 0.1.0
**Status**: Alpha
**License**: MIT
