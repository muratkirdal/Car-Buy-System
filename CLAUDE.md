# CLAUDE.md

Vehicle purchase system (NFS Underground 2 style) - 3D showcase, carousel browsing, color customization, currency management.

**Unity 2022.3.62f2 | URP 14.0.12 | MVP + Service Layer**

## Project Structure

| Path | Purpose |
|------|---------|
| `Scripts/Core/` | `GameBootstrap` - entry point |
| `Scripts/Data/` | ScriptableObjects (VehicleData, VehicleLibrary, ShopSettings) |
| `Scripts/Services/` | Business logic (VehicleService, CurrencyService, TransactionService) |
| `Scripts/UI/` | Views + `ShopController` (MVP presenter) |
| `Scripts/Vehicle/` | 3D display (`VehicleShowcase`, `VehicleDisplayInstance`) |

All paths relative to `Assets/CarBuy/`.

## Key Systems

| System | Entry Point | Notes |
|--------|-------------|-------|
| Transaction | `ITransactionService.PurchaseVehicle()` | Returns `TransactionResult` enum |
| 3D Display | `VehicleShowcase` | Uses `MaterialPropertyBlock` (no material instances) |
| UI State | `ShopUIState` | Tracks vehicle index, color, popup state |

## Data Flow

1. `GameBootstrap` → `ShopController.Initialize()` → Creates services, wires events
2. Purchase → `ConfirmationPopup` → `ITransactionService` → `PurchaseCompleted` event → Updates views

## Conventions

- Private fields: `m_CamelCase`
- Events: null-conditional invoke (`Event?.Invoke()`)
- Use `MaterialPropertyBlock` for color changes (avoid material instances)
- Kill DOTween sequences/tweens in `OnDisable`

## Pitfalls

| Issue | Solution |
|-------|----------|
| Material memory leaks | Use `MaterialPropertyBlock` |
| DOTween leaks | Kill in `OnDisable` |
| Services null | Call `ShopController.Initialize()` first |

## Dependencies (non-standard)

| Package | Use |
|---------|-----|
| DOTween | UI animations, vehicle transitions |
| UniTask | Async operations |
| `unity-mcp` | Claude Code integration |
