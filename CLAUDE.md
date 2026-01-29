# CLAUDE.md

Vehicle purchase system (NFS Underground 2 style) - 3D showcase, carousel browsing, color customization, currency management.

**Unity 2022.3.62f2 | URP 14.0.12 | MVP + Service Layer**

## Project Structure

| Path | Purpose |
|------|---------|
| `Scripts/Core/` | ServiceLocator, GameEvents |
| `Scripts/Data/` | ScriptableObjects (VehicleData, VehicleCatalog, PlayerShopData) |
| `Scripts/Services/` | Business logic (VehicleService, CurrencyService, TransactionService) |
| `Scripts/UI/` | Views + ShopController (MVP presenter) |
| `Scripts/Vehicle/` | 3D display (VehicleShowcase, VehicleDisplayInstance) |
| `Tests/Editor/` | EditMode tests |
| `Tests/Runtime/` | PlayMode tests |
| `Documentation/` | GDD + TDD specs |

## Assemblies

| Assembly | References |
|----------|------------|
| `CarBuy` | TextMeshPro, URP Runtime, Input System |
| `CarBuy.Tests` | CarBuy, TestRunner, NUnit |

## Key Systems

| System | Entry Point | Notes |
|--------|-------------|-------|
| Service Locator | `ServiceLocator.Register<T>/Get<T>` | Clear in test teardown |
| Vehicle Catalog | `VehicleCatalog.asset` | Single source of truth |
| Transaction | `ITransactionService` | Returns `TransactionResult` enum |
| 3D Display | `VehicleShowcase` | Uses `MaterialPropertyBlock` (no material instances) |
| UI State | `ShopUIState` | Tracks vehicle index, color, popup state |

## Data Flow

1. Input → `ShopController` → Views (CarouselView, StatsPanelView, PurchasePanelView)
2. Purchase → `ConfirmationPopup` → `ITransactionService` → Updates all views

## Conventions

- Private fields: `_camelCase`
- Interfaces: `IName` prefix
- Events: `OnEventName` prefix, null-conditional invoke
- Serialized fields: `[SerializeField][Tooltip]`
- Use `MaterialPropertyBlock` for color changes (avoid material instances)

## Testing

```csharp
[TearDown] public void TearDown() => ServiceLocator.Clear();  // Required
```

Mock services via interface implementations, register in `[SetUp]`.

## Common Pitfalls

| Issue | Solution |
|-------|----------|
| Tests fail with stale services | `ServiceLocator.Clear()` in TearDown |
| Material memory leaks | Use `MaterialPropertyBlock` |
| "Type not found" in tests | Check `CarBuy.Tests.asmdef` references |
| Services null | Ensure `ShopController.Initialize()` called first |

## Dependencies (non-standard)

| Package | Use |
|---------|-----|
| `unity-mcp` | Claude Code integration |
| `com.unity.timeline` | Animation sequences |
