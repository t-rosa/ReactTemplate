# Quick Start - ReactTemplate.Tools CLI 🚀

## ⚡ 30-second TL;DR

```bash
# Installer
dotnet tool install --global --add-source ./ReactTemplate.Tools/bin/Debug ReactTemplate.Tools

# Tester
react-template-scaffold scaffold \
  --module Products \
  --entity Product \
  --fields "name:string required; price:decimal; stock:int" \
  --dry-run
```

## 📂 Où sont les fichiers?

```
c:/Users/tomar/source/repos/ReactTemplate/
├── ReactTemplate.Tools/          ← CLI PROJECT
│   ├── Program.cs               (Entry + CLI args)
│   ├── Services/
│   │   ├── RepositoryDetector.cs
│   │   ├── FieldParser.cs
│   │   ├── SpecFileLoader.cs
│   │   ├── TemplateRenderer.cs
│   │   └── ScaffoldingService.cs (✋ INCOMPLETE - renders but doesn't write)
│   ├── Models/
│   │   └── EntitySchema.cs
│   ├── Commands/
│   │   └── ScaffoldOptions.cs
│   ├── Templates/               (Scriban templates)
│   │   ├── server-entity.scriban
│   │   ├── server-dtos.scriban
│   │   ├── server-validators.scriban
│   │   ├── server-controller.scriban
│   │   ├── client-hooks.scriban
│   │   ├── client-form.scriban
│   │   └── example.product.json
│   ├── README.md                (Usage doc)
│   ├── PROGRESS.md              (Phase 1 summary)
│   └── ROADMAP.md               (Phase 2-5 plan)
│
├── docs/
│   └── scaffolding.md           (Complete user documentation)
```

## ✅ Ce qui marche déjà

- ✅ CLI parsing (`--module`, `--entity`, `--fields`, etc.)
- ✅ Repository detection (cherche `ReactTemplate.slnx`)
- ✅ Field parsing (`name:string required; price:decimal`)
- ✅ JSON/YAML spec loading
- ✅ Scriban template rendering (placeholders)
- ✅ Dry-run preview (affiche ce qui sera généré)
- ✅ Build & compilation (pas d'erreurs)

## ⏳ À faire NEXT (Phase 2)

Ces méthodes dans `ScaffoldingService` sont skeleton:

```csharp
GenerateEntityFileAsync()      // Rendre + écrire Entity.cs
GenerateDtosAsync()            // Rendre + écrire DTOs
GenerateControllerAsync()      // Rendre + écrire Controller.cs
GenerateValidatorsAsync()      // Rendre + écrire Validators.cs
GenerateHooksAsync()           // Rendre + écrire hooks.ts
GenerateFormAsync()            // Rendre + écrire form.tsx
GenerateViewsAsync()           // Créer views list/detail/edit
FormatCSharpFilesAsync()       // Exécuter dotnet format
FormatTypeScriptFilesAsync()   // Exécuter prettier
```

## 🧪 Tester maintenant

```bash
cd /c/Users/tomar/source/repos/ReactTemplate

# Dry-run simple
dotnet run --project ReactTemplate.Tools -- scaffold \
  --module Products \
  --entity Product \
  --dry-run

# Dry-run avec spec
dotnet run --project ReactTemplate.Tools -- scaffold \
  --spec ReactTemplate.Tools/Templates/example.product.json \
  --dry-run
```

## 🎯 Priorités prochaines étapes

1. **FileWriter.cs**: Service pour écrire fichiers de manière sûre
2. **Implémenter Generate\*Async()**: Rendre templates + écrire fichiers
3. **DbContextModifier.cs**: Ajouter DbSet & ModelConfig auto
4. **Tests**: Vérifier génération fichiers corrects
5. **Publish**: dotnet pack + NuGet

## 🔗 Documentation

- **User Guide**: `docs/scaffolding.md` (COMPLET)
- **Architecture**: `ReactTemplate.Tools/README.md`
- **Progress**: `ReactTemplate.Tools/PROGRESS.md`
- **Roadmap**: `ReactTemplate.Tools/ROADMAP.md`

## 💾 Exemple fichier spec JSON

```json
{
  "entityName": "Product",
  "moduleName": "Products",
  "hasAuditFields": true,
  "fields": [
    {
      "name": "Name",
      "type": "string",
      "required": true
    },
    {
      "name": "Price",
      "type": "decimal",
      "required": true
    }
  ]
}
```

## 🚨 Problèmes connus

- [ ] Fichiers pas écrits (skeleton methods)
- [ ] DbContext pas modifié auto (TODO)
- [ ] Migrations pas suggérées (TODO)
- [ ] Tests scaffolds pas générés (TODO)

## 📞 Questions?

Voir `ROADMAP.md` section "Learning Resources"

---

**Status**: Phase 1 Complete ✨ | Phase 2 Ready to Start 🚀
