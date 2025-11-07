# Roadmap - Itération 2 & 3 🗺️

## Phase 2: Génération réelle de fichiers (Itération 2)

### Objectifs

Implémenter la génération réelle de fichiers serveur et client à partir des templates Scriban.

### Tâches

#### 1. ScaffoldingService - Génération Serveur

```csharp
// À implémenter dans Services/ScaffoldingService.cs

private async Task<bool> GenerateEntityFileAsync(EntitySchema schema, string baseDir)
{
    // Utiliser TemplateRenderer pour rendre server-entity.scriban
    // Écrire le résultat dans {baseDir}/{Entity}.cs
}

private async Task<bool> GenerateDtosAsync(EntitySchema schema, string baseDir)
{
    // Rendre server-dtos.scriban
    // Créer Dtos/{Entity}DTOs.cs
}

private async Task<bool> GenerateControllerAsync(EntitySchema schema, string baseDir)
{
    // Rendre server-controller.scriban
    // Créer {Entity}Controller.cs
}

private async Task<bool> GenerateValidatorsAsync(EntitySchema schema, string baseDir)
{
    // Rendre server-validators.scriban
    // Créer Validators/{Entity}Validators.cs
}
```

#### 2. ScaffoldingService - Génération Client

```csharp
private async Task<bool> GenerateHooksAsync(EntitySchema schema, string baseDir)
{
    // Rendre client-hooks.scriban
    // Écrire hooks.ts
}

private async Task<bool> GenerateFormAsync(EntitySchema schema, string baseDir)
{
    // Rendre client-form.scriban
    // Écrire form.tsx
}

private async Task<bool> GenerateViewsAsync(EntitySchema schema, string baseDir)
{
    // Créer views/list.tsx
    // Créer views/detail.tsx
    // Créer views/create-edit.tsx
    // Créer index.ts barrel export
}
```

#### 3. Intégration DbContext

Service pour modifier automatiquement `ApplicationDbContext.cs`:

```csharp
public class DbContextModifier
{
    public void AddDbSet(string contextPath, EntitySchema schema)
    {
        // Ajouter ligne: public DbSet<{Entity}> {entity-slug} { get; set; }
        // Si n'existe pas déjà
    }

    public void AddModelConfiguration(string contextPath, EntitySchema schema)
    {
        // Ajouter OnModelCreating:
        // builder.Entity<{Entity}>(entity => { ... });
    }
}
```

#### 4. Tests Unit Serveur

Template `server-tests.scriban`:

```csharp
public class {{EntityName}}ControllerTests
{
    [Fact]
    public async Task Get_ReturnsOkWithList()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedAtAction()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

#### 5. Tests Client

- Template `client-vitest.scriban`: Tests unitaires hooks + form
- Template `client-e2e.scriban`: Tests Playwright (CRUD complet)

### Fichiers à créer

- `Services/FileWriter.cs`: Écriture fichiers sécurisée
- `Services/DbContextModifier.cs`: Modification ApplicationDbContext
- `Templates/client-views.scriban`: Views list/detail/form
- `Templates/server-tests.scriban`: Tests xUnit
- `Templates/client-vitest.scriban`: Tests Vitest
- `Templates/client-e2e.scriban`: Tests Playwright

### Tests d'acceptance

```bash
# Générer Product
react-template-scaffold scaffold -m Products -e Product -f "name:string required; price:decimal"

# Vérifier fichiers créés
test -f ReactTemplate.Server/Modules/Products/Product/Product.cs
test -f ReactTemplate.Server/Modules/Products/Product/ProductController.cs
test -d ReactTemplate.Server/Modules/Products/Product/Dtos
test -d ReactTemplate.Server/Modules/Products/Product/Validators
test -f ReactTemplate.Client/src/modules/products/product/hooks.ts
test -f ReactTemplate.Client/src/modules/products/product/form.tsx

# Vérifier DbContext modifié
grep -q "public DbSet<Product> Products" ReactTemplate.Server/ApplicationDbContext.cs

# Build réussit
dotnet build ReactTemplate.Server
npm run build # dans ReactTemplate.Client
```

---

## Phase 3: Post-génération & Hooks (Itération 3)

### Objectifs

- Formatter automatiquement
- Suggérer & automatiser migrations EF
- Intégrer pre-commit hooks Husky

### 3.1 Formatting & Linting

```csharp
public class PostGenerationService
{
    public async Task FormatCSharpAsync(string repoRoot)
    {
        // dotnet format --folder {repoRoot}/ReactTemplate.Server
    }

    public async Task FormatTypeScriptAsync(string repoRoot)
    {
        // npm run lint -- --fix dans {repoRoot}/ReactTemplate.Client
        // OU prettier --write src/
    }

    public async Task SuggestAndApplyMigrationAsync(EntitySchema schema, string repoRoot)
    {
        // Afficher: dotnet ef migrations add Add{Entity}
        // Si --auto-migrate: exécuter directement
    }
}
```

### 3.2 Husky Initialization

Service pour configurer hooks git:

```csharp
public class HuskySetupService
{
    public void InitializeHusky(string repoRoot)
    {
        // npx husky-init --yarn (ou npm)
        // Créer .husky/pre-commit
        // Créer .husky/prepare-commit-msg
    }

    public void ConfigureLintStaged(string repoRoot)
    {
        // Créer .lintstagedrc.json avec:
        // "*.cs": ["dotnet format"],
        // "*.{ts,tsx}": ["prettier --write", "eslint --fix"]
    }
}
```

### 3.3 Commande Setup

```bash
react-template-scaffold setup [--auto-format] [--auto-migrate] [--setup-husky]
```

---

## Phase 4: Testing & Publication (Itération 4)

### 4.1 Unit Tests pour CLI

```csharp
// Tests/FieldParserTests.cs
[Fact]
public void ParseFieldsString_WithValidInput_ReturnsCorrectFields()
{
    var input = "name:string required; price:decimal; stock:int";
    var result = _parser.ParseFieldsString(input);
    Assert.Equal(3, result.Count);
}

// Tests/RepositoryDetectorTests.cs
[Fact]
public void DetectRepositoryRoot_FindsSlnFile()
{
    var result = _detector.DetectRepositoryRoot();
    Assert.NotNull(result);
    Assert.True(File.Exists(Path.Combine(result, "ReactTemplate.slnx")));
}

// Tests/TemplateRendererTests.cs
[Fact]
public void RenderTemplate_ReplacesPlaceholders()
{
    var result = _renderer.RenderTemplate("...", schema);
    Assert.Contains("Product", result);
    Assert.Contains("product-singular", result);
}
```

### 4.2 Integration Tests

```bash
# Test end-to-end
./test-e2e.sh
  - Générer Product entity
  - Vérifier fichiers créés
  - Build serveur
  - Build client
  - Run tests générés
```

### 4.3 Publication NuGet

```bash
# Build
dotnet pack ReactTemplate.Tools

# Push to NuGet
dotnet nuget push ReactTemplate.Tools/nupkg/ReactTemplate.Tools.0.1.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

---

## Phase 5: Advanced Features (Itération 5+)

### 5.1 Relations 1-N (future)

```yaml
# spec.yaml
entityName: Order
fields:
  - name: Products
    type: ICollection<OrderItem>
    isRelation: true
    relationTarget: OrderItem
    relationCardinality: one-to-many
```

### 5.2 Relations N-N (future)

```yaml
entityName: Student
fields:
  - name: Courses
    type: ICollection<Course>
    isRelation: true
    isManyToMany: true
```

### 5.3 Enums (future)

```yaml
fields:
  - name: Status
    type: enum
    enumValues:
      - Active
      - Inactive
      - Pending
```

### 5.4 Owned Types (future)

```yaml
fields:
  - name: Address
    type: owned
    ownedType:
      fields:
        - Street: string
        - City: string
        - ZipCode: string
```

---

## 📌 Priorités

**High Priority (V0.2)**

1. ✅ Génération fichiers serveur (Entity, Controller, DTOs, Validators)
2. ✅ Génération fichiers client (hooks, form)
3. ✅ DbContext modification automatique
4. ✅ Post-generation formatting
5. ✅ Migration EF suggestion

**Medium Priority (V0.3)**

1. Tests xUnit scaffold
2. Tests Vitest scaffold
3. Tests Playwright scaffold
4. Husky + lint-staged setup
5. Publication NuGet

**Low Priority (V1.0+)**

1. Relations N-N
2. Enums
3. Owned types
4. Soft-delete patterns
5. Multi-database support

---

## 🎓 Learning Resources

### Scriban Templating

- [Scriban Language](https://github.com/scriban/scriban/blob/master/doc/language.md)
- [Liquid filters in Scriban](https://github.com/scriban/scriban/blob/master/doc/builtins.md)

### EF Core

- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

### React Hook Form

- [Documentation](https://react-hook-form.com/)
- [Examples](https://github.com/react-hook-form/react-hook-form/tree/master/examples)

### Husky

- [Husky Docs](https://typicode.github.io/husky/)
- [Pre-commit Hooks](https://typicode.github.io/husky/guide.html)

---

## 📋 Checklist de déploiement

- [ ] Toutes les tâches Phase 2 complètes
- [ ] Tests unitaires (>80% coverage)
- [ ] Tests end-to-end passants
- [ ] Documentation mise à jour
- [ ] Changelog rédigé
- [ ] Version bumped (semver)
- [ ] NuGet package publié
- [ ] GitHub release créée
- [ ] README.md principal updated
- [ ] Annonce sur discussions

---

**Next Review**: Après Phase 2 completion
**Estimated Timeline**: 2-3 semaines par phase
**Maintainer**: @t-rosa
