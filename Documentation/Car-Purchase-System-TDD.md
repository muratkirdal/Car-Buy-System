# Car Purchase System - Technical Design Document

## Document Information

| Field | Value |
|-------|-------|
| **Document Title** | Car Purchase System TDD |
| **Version** | 1.0 |
| **Date** | January 28, 2026 |
| **Unity Version** | 2022.3.62f2 |
| **Render Pipeline** | Universal Render Pipeline (URP) 14.0.12 |
| **Source GDD** | `/Users/muratkirdal/CarBuy/Documentation/Car-Purchase-System-GDD.md` |

---

## 1. Scope Analysis

### 1.1 What's Being Built

A complete vehicle purchase interface system inspired by Need for Speed Underground 2, featuring:
- Horizontal carousel for vehicle browsing with infinite wrapping
- 3D vehicle display on a rotating platform with dynamic lighting
- Stats panel showing performance metrics with animated sliders
- Color selection system with real-time vehicle material updates
- Purchase flow with confirmation popup and transaction handling
- Multi-platform input support (Controller, Keyboard/Mouse, Touch)

### 1.2 Boundaries

**In Scope:**
- Complete UI system for vehicle browsing and purchasing
- Vehicle data architecture (ScriptableObjects)
- 3D vehicle showcase with camera system
- Purchase transaction logic with currency management
- Input handling for multiple platforms
- Audio feedback system integration points
- Save/Load hooks for purchase persistence

**Out of Scope:**
- Actual vehicle 3D models (placeholder system provided)
- Garage system (interface only provided)
- Racing/gameplay integration
- Network multiplayer transactions
- Microtransaction/real-money handling
- Localization implementation (structure provided)

### 1.3 Success Criteria

| Criteria | Target |
|----------|--------|
| Navigation Understanding | < 5 seconds for new users |
| Purchase Flow Completion | < 30 seconds from selection |
| Frame Rate (PC) | 60+ FPS |
| Frame Rate (Mobile) | 30+ FPS |
| Vehicle Transition Time | 500ms |
| UI Response Time | < 100ms |
| Memory Footprint | < 512 MB |

---

## 2. Technology Stack

### 2.1 Unity Features

| Feature | Usage |
|---------|-------|
| **UI Toolkit / uGUI** | uGUI (Canvas-based) for UI panels and carousel |
| **Universal Render Pipeline** | 3D vehicle rendering with custom lighting |
| **Timeline** | Vehicle transition animations |
| **TextMeshPro** | All text rendering |
| **Animation** | UI animations and vehicle effects |
| **New Input System** | Recommended for multi-platform input |

### 2.2 Required Packages

| Package | Version | Status | Purpose |
|---------|---------|--------|---------|
| `com.unity.render-pipelines.universal` | 14.0.12 | Installed | 3D rendering |
| `com.unity.textmeshpro` | 3.0.7 | Installed | Text rendering |
| `com.unity.ugui` | 1.0.0 | Installed | UI system |
| `com.unity.inputsystem` | 1.7.0+ | **Required** | Multi-platform input |
| `com.unity.cinemachine` | 2.9.7+ | **Recommended** | Camera management |
| `DOTween` | 1.0.x | **Recommended** | Animation tweening |

### 2.3 Platform Considerations

| Platform | Considerations |
|----------|----------------|
| **PC** | Ultrawide support, high-detail LOD, keyboard/mouse/controller |
| **PlayStation** | DualSense haptics, adaptive triggers, Circle=Back |
| **Xbox** | Controller focus, B=Back, achievement hooks |
| **Mobile** | Touch carousel, portrait layout option, battery optimization |

---

## 3. Architecture Design

### 3.1 Pattern Selection

**Primary Pattern: Model-View-Presenter (MVP) with Service Layer**

```
+------------------+     +------------------+     +------------------+
|     SERVICES     |     |    PRESENTERS    |     |      VIEWS       |
|------------------|     |------------------|     |------------------|
| VehicleService   |<--->| ShopPresenter    |<--->| ShopView         |
| CurrencyService  |     | CarouselPresenter|     | CarouselView     |
| TransactionSvc   |     | StatsPresenter   |     | StatsPanelView   |
| AudioService     |     | PurchasePresenter|     | PurchaseView     |
+------------------+     +------------------+     +------------------+
         ^                        ^
         |                        |
+------------------+     +------------------+
|      MODELS      |     |   CONTROLLERS    |
|------------------|     |------------------|
| VehicleData (SO) |     | ShopController   |
| PlayerData       |     | InputController  |
| TransactionData  |     | CameraController |
+------------------+     +------------------+
```

### 3.2 Justification

- **MVP**: Separates UI logic from business logic, enabling unit testing of presenters
- **Service Layer**: Encapsulates data access and business operations, easily mockable
- **ScriptableObjects**: Data-driven vehicle definitions, designer-friendly
- **Event-Driven**: Loose coupling between systems via C# events/delegates

### 3.3 Namespace Organization

```
CarBuy/
├── Core/                    # Base classes, interfaces, utilities
├── Data/                    # ScriptableObjects and data models
├── Services/                # Business logic services
├── UI/                      # UI views and presenters
│   ├── Carousel/
│   ├── Stats/
│   ├── Purchase/
│   └── Common/
├── Vehicle/                 # Vehicle display and management
├── Input/                   # Input handling
├── Audio/                   # Audio management
└── Camera/                  # Camera system
```

---

## 4. Class Design

### 4.1 Core Data Classes

#### VehicleData (ScriptableObject)

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Data/VehicleData.cs`

```csharp
namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "Vehicle", menuName = "CarBuy/Vehicle Data")]
    public class VehicleData : ScriptableObject
    {
        // Identity
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public string Manufacturer { get; private set; }
        [field: SerializeField] public VehicleClass Class { get; private set; }
        
        // Economics
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public bool IsLocked { get; private set; }
        [field: SerializeField] public string UnlockCondition { get; private set; }
        
        // Performance Stats (0-100)
        [field: SerializeField] public VehicleStats Stats { get; private set; }
        
        // Visuals
        [field: SerializeField] public VehicleColorOption[] Colors { get; private set; }
        [field: SerializeField] public VehicleModelInfo ModelInfo { get; private set; }
        [field: SerializeField] public Sprite PreviewIcon { get; private set; }
    }
}
```

#### VehicleStats (Serializable Struct)

```csharp
namespace CarBuy.Data
{
    [System.Serializable]
    public struct VehicleStats
    {
        [Range(0, 100)] public int Speed;
        [Range(0, 100)] public int Acceleration;
        [Range(0, 100)] public int Handling;
    }
}
```

#### VehicleColorOption (Serializable Class)

```csharp
namespace CarBuy.Data
{
    [System.Serializable]
    public class VehicleColorOption
    {
        public string Id;
        public string Name;
        public Color Color;
        public MaterialType MaterialType;
        public Material MaterialOverride; // Optional custom material
    }
}
```

#### PlayerShopData (Runtime Data)

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Data/PlayerShopData.cs`

```csharp
namespace CarBuy.Data
{
    [System.Serializable]
    public class PlayerShopData
    {
        public int Balance;
        public List<OwnedVehicle> OwnedVehicles = new();
        
        public bool OwnsVehicle(string vehicleId) 
            => OwnedVehicles.Any(v => v.VehicleId == vehicleId);
    }
    
    [System.Serializable]
    public class OwnedVehicle
    {
        public string VehicleId;
        public string SelectedColorId;
        public long PurchaseTimestamp;
    }
}
```

---

### 4.2 Service Classes

#### IVehicleService (Interface)

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Services/IVehicleService.cs`

```csharp
namespace CarBuy.Services
{
    public interface IVehicleService
    {
        IReadOnlyList<VehicleData> GetAllVehicles();
        VehicleData GetVehicle(string id);
        bool IsVehicleUnlocked(string id);
        bool IsVehicleOwned(string id);
    }
}
```

#### ICurrencyService (Interface)

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Services/ICurrencyService.cs`

```csharp
namespace CarBuy.Services
{
    public interface ICurrencyService
    {
        int CurrentBalance { get; }
        bool CanAfford(int amount);
        bool TryDeduct(int amount);
        void Add(int amount);
        event Action<int> OnBalanceChanged;
    }
}
```

#### ITransactionService (Interface)

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Services/ITransactionService.cs`

```csharp
namespace CarBuy.Services
{
    public interface ITransactionService
    {
        Task<TransactionResult> PurchaseVehicle(string vehicleId, string colorId);
        event Action<PurchaseTransaction> OnPurchaseCompleted;
        event Action<PurchaseTransaction, string> OnPurchaseFailed;
    }
    
    public enum TransactionResult
    {
        Success,
        InsufficientFunds,
        AlreadyOwned,
        VehicleLocked,
        ServerError
    }
}
```

#### VehicleService (Implementation)

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Services/VehicleService.cs`

```csharp
namespace CarBuy.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly VehicleCatalog _catalog;
        private readonly PlayerShopData _playerData;
        
        public VehicleService(VehicleCatalog catalog, PlayerShopData playerData)
        {
            _catalog = catalog;
            _playerData = playerData;
        }
        
        public IReadOnlyList<VehicleData> GetAllVehicles() => _catalog.Vehicles;
        public VehicleData GetVehicle(string id) => _catalog.GetById(id);
        public bool IsVehicleUnlocked(string id) => !_catalog.GetById(id).IsLocked;
        public bool IsVehicleOwned(string id) => _playerData.OwnsVehicle(id);
    }
}
```

---

### 4.3 UI Components

#### ShopController (Main Controller)

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/UI/ShopController.cs`

**Responsibility:** Orchestrates all shop subsystems and manages shop lifecycle.

```csharp
namespace CarBuy.UI
{
    public class ShopController : MonoBehaviour
    {
        // Dependencies (injected via Inspector or DI)
        [SerializeField] private VehicleCatalog _vehicleCatalog;
        [SerializeField] private CarouselView _carouselView;
        [SerializeField] private StatsPanelView _statsView;
        [SerializeField] private PurchasePanelView _purchaseView;
        [SerializeField] private ConfirmationPopup _confirmationPopup;
        [SerializeField] private VehicleShowcase _vehicleShowcase;
        
        // Services
        private IVehicleService _vehicleService;
        private ICurrencyService _currencyService;
        private ITransactionService _transactionService;
        
        // State
        private ShopUIState _state;
        
        // Public API
        public void Initialize(PlayerShopData playerData);
        public void OpenShop();
        public void CloseShop();
        
        // Events
        public event Action OnShopOpened;
        public event Action OnShopClosed;
        public event Action<VehicleData> OnVehiclePurchased;
    }
}
```

#### CarouselView

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/UI/Carousel/CarouselView.cs`

**Responsibility:** Displays and animates the vehicle selection carousel.

```csharp
namespace CarBuy.UI.Carousel
{
    public class CarouselView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _container;
        [SerializeField] private CarouselItem _itemPrefab;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        
        [Header("Settings")]
        [SerializeField] private int _visibleItems = 7;
        [SerializeField] private float _transitionDuration = 0.3f;
        [SerializeField] private AnimationCurve _transitionCurve;
        
        // Public API
        public void Initialize(IReadOnlyList<VehicleData> vehicles);
        public void SelectIndex(int index, bool animate = true);
        public void NavigateLeft();
        public void NavigateRight();
        
        // Events
        public event Action<int, VehicleData> OnVehicleSelected;
        public event Action<int, VehicleData> OnVehicleHovered;
        
        // State
        public int CurrentIndex { get; private set; }
        public VehicleData CurrentVehicle { get; private set; }
    }
}
```

#### StatsPanelView

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/UI/Stats/StatsPanelView.cs`

**Responsibility:** Displays vehicle stats and color selection.

```csharp
namespace CarBuy.UI.Stats
{
    public class StatsPanelView : MonoBehaviour
    {
        [Header("Header")]
        [SerializeField] private TMP_Text _vehicleNameText;
        [SerializeField] private Image _classIcon;
        
        [Header("Stats")]
        [SerializeField] private StatSlider _speedSlider;
        [SerializeField] private StatSlider _accelerationSlider;
        [SerializeField] private StatSlider _handlingSlider;
        
        [Header("Colors")]
        [SerializeField] private ColorButton _colorButtonPrefab;
        [SerializeField] private Transform _colorContainer;
        
        [Header("Animation")]
        [SerializeField] private float _statFillDuration = 0.4f;
        
        // Public API
        public void DisplayVehicle(VehicleData vehicle, bool isOwned);
        public void DisplayLocked(VehicleData vehicle);
        public void SelectColor(int colorIndex);
        
        // Events
        public event Action<int, VehicleColorOption> OnColorSelected;
    }
}
```

#### PurchasePanelView

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/UI/Purchase/PurchasePanelView.cs`

**Responsibility:** Displays price and purchase button.

```csharp
namespace CarBuy.UI.Purchase
{
    public class PurchasePanelView : MonoBehaviour
    {
        [Header("Display")]
        [SerializeField] private TMP_Text _vehicleNameText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _balanceText;
        [SerializeField] private GameObject _saleIndicator;
        [SerializeField] private TMP_Text _originalPriceText;
        
        [Header("Buttons")]
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private TMP_Text _purchaseButtonText;
        [SerializeField] private GameObject _ownedBadge;
        
        [Header("Colors")]
        [SerializeField] private Color _affordableColor = Color.green;
        [SerializeField] private Color _unaffordableColor = Color.red;
        
        // Public API
        public void DisplayVehicle(VehicleData vehicle, int playerBalance, bool isOwned);
        public void SetPurchaseEnabled(bool enabled);
        public void ShowProcessing(bool processing);
        
        // Events
        public event Action OnPurchaseClicked;
        public event Action OnViewInGarageClicked;
    }
}
```

#### ConfirmationPopup

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/UI/Common/ConfirmationPopup.cs`

**Responsibility:** Modal confirmation dialog for purchases.

```csharp
namespace CarBuy.UI.Common
{
    public class ConfirmationPopup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private Image _backgroundOverlay;
        
        [Header("Animation")]
        [SerializeField] private float _fadeInDuration = 0.2f;
        [SerializeField] private float _fadeOutDuration = 0.15f;
        
        // Public API
        public Task<bool> ShowAsync(string vehicleName, int price);
        public void ForceClose();
        
        // Events
        public event Action<bool> OnClosed;
    }
}
```

---

### 4.4 Vehicle Display Classes

#### VehicleShowcase

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Vehicle/VehicleShowcase.cs`

**Responsibility:** Manages 3D vehicle display, rotation, and transitions.

```csharp
namespace CarBuy.Vehicle
{
    public class VehicleShowcase : MonoBehaviour
    {
        [Header("Platform")]
        [SerializeField] private Transform _platformTransform;
        [SerializeField] private Transform _vehicleSpawnPoint;
        [SerializeField] private float _rotationSpeed = 10f;
        
        [Header("Transition")]
        [SerializeField] private float _transitionDuration = 0.5f;
        [SerializeField] private AnimationCurve _fadeOutCurve;
        [SerializeField] private AnimationCurve _fadeInCurve;
        
        [Header("Lighting")]
        [SerializeField] private Light _keyLight;
        [SerializeField] private Light _fillLight;
        [SerializeField] private Light _rimLight;
        
        // State
        private VehicleDisplayInstance _currentVehicle;
        private bool _isRotating = true;
        
        // Public API
        public void DisplayVehicle(VehicleData vehicle, int colorIndex = 0);
        public void SetColor(VehicleColorOption colorOption);
        public void SetRotation(bool rotating);
        public void PauseRotation();
        public void ResumeRotation();
        
        // Events
        public event Action OnTransitionStarted;
        public event Action OnTransitionCompleted;
    }
}
```

#### VehicleDisplayInstance

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Vehicle/VehicleDisplayInstance.cs`

**Responsibility:** Individual vehicle instance for display.

```csharp
namespace CarBuy.Vehicle
{
    public class VehicleDisplayInstance : MonoBehaviour
    {
        [SerializeField] private Renderer[] _paintableRenderers;
        [SerializeField] private string _colorPropertyName = "_BaseColor";
        
        private MaterialPropertyBlock _propertyBlock;
        
        public void SetColor(Color color);
        public void SetMaterial(Material material);
        public void FadeIn(float duration);
        public void FadeOut(float duration);
    }
}
```

---

### 4.5 Input System

#### ShopInputHandler

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Input/ShopInputHandler.cs`

**Responsibility:** Handles all input for the shop system.

```csharp
namespace CarBuy.Input
{
    public class ShopInputHandler : MonoBehaviour
    {
        [Header("Input Settings")]
        [SerializeField] private float _holdNavigationDelay = 0.5f;
        [SerializeField] private float _holdNavigationRate = 0.1f;
        [SerializeField] private float _inputDebounceTime = 0.1f;
        
        // Events
        public event Action OnNavigateLeft;
        public event Action OnNavigateRight;
        public event Action OnSelect;
        public event Action OnCancel;
        public event Action OnQuickPurchase;
        public event Action OnColorCycleLeft;
        public event Action OnColorCycleRight;
        
        // State
        public bool IsInputEnabled { get; set; } = true;
        
        // Input Action references (New Input System)
        private ShopInputActions _inputActions;
    }
}
```

---

## 5. Data Flow

### 5.1 Vehicle Selection Flow

```
User Input (Navigate)
        |
        v
ShopInputHandler.OnNavigateLeft/Right
        |
        v
ShopController.HandleNavigation()
        |
        +---> CarouselView.SelectIndex()
        |           |
        |           v
        |     Animate carousel items
        |           |
        |           v
        |     Fire OnVehicleSelected event
        |
        +---> StatsPanelView.DisplayVehicle()
        |           |
        |           v
        |     Animate stat sliders
        |     Populate color buttons
        |
        +---> PurchasePanelView.DisplayVehicle()
        |           |
        |           v
        |     Update price display
        |     Update button state
        |
        +---> VehicleShowcase.DisplayVehicle()
                    |
                    v
              Fade out current vehicle
              Load new vehicle prefab
              Fade in new vehicle
              Resume platform rotation
```

### 5.2 Purchase Flow

```
User clicks Purchase Button
        |
        v
PurchasePanelView.OnPurchaseClicked
        |
        v
ShopController.HandlePurchaseRequest()
        |
        v
ConfirmationPopup.ShowAsync()
        |
        +---> User selects NO ---> Return to browsing
        |
        +---> User selects YES
                    |
                    v
        TransactionService.PurchaseVehicle()
                    |
                    +---> Deduct currency
                    |
                    +---> Add to owned vehicles
                    |
                    +---> Save player data
                    |
                    v
        Fire OnPurchaseCompleted event
                    |
                    +---> CarouselView.UpdateOwnedState()
                    +---> PurchasePanelView.ShowOwned()
                    +---> AudioService.PlayPurchaseSuccess()
                    +---> Show success notification
```

### 5.3 State Management

```csharp
namespace CarBuy.UI
{
    [System.Serializable]
    public class ShopUIState
    {
        public int CurrentVehicleIndex;
        public int SelectedColorIndex;
        public bool IsPopupOpen;
        public bool IsProcessingPurchase;
        public string ErrorMessage;
        
        public VehicleData CurrentVehicle { get; set; }
        public bool CanPurchase => 
            !IsPopupOpen && 
            !IsProcessingPurchase && 
            CurrentVehicle != null;
    }
}
```

---

## 6. Integration Strategy

### 6.1 Service Locator / Dependency Injection

For this project, we will use a simple Service Locator pattern with optional support for external DI frameworks.

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Core/ServiceLocator.cs`

```csharp
namespace CarBuy.Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();
        
        public static void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }
        
        public static T Get<T>() where T : class
        {
            return _services.TryGetValue(typeof(T), out var service) 
                ? (T)service 
                : throw new InvalidOperationException($"Service {typeof(T)} not registered");
        }
        
        public static void Clear() => _services.Clear();
    }
}
```

### 6.2 Event System

**File:** `/Users/muratkirdal/CarBuy/Assets/CarBuy/Scripts/Core/GameEvents.cs`

```csharp
namespace CarBuy.Core
{
    public static class GameEvents
    {
        // Shop Events
        public static event Action OnShopOpened;
        public static event Action OnShopClosed;
        
        // Vehicle Events
        public static event Action<VehicleData> OnVehicleSelected;
        public static event Action<VehicleData, VehicleColorOption> OnColorChanged;
        public static event Action<VehicleData> OnVehiclePurchased;
        
        // Currency Events
        public static event Action<int> OnCurrencyChanged;
        
        // Invoke methods...
    }
}
```

### 6.3 Save System Integration Points

The system provides hooks for external save systems:

```csharp
namespace CarBuy.Services
{
    public interface ISaveProvider
    {
        Task<PlayerShopData> LoadPlayerData();
        Task SavePlayerData(PlayerShopData data);
        Task<bool> HasSaveData();
    }
    
    // Default implementation using PlayerPrefs/JSON
    public class LocalSaveProvider : ISaveProvider { ... }
}
```

---

## 7. Implementation Phases

### Phase 1: Foundation (Complexity: M)
**Duration:** 2-3 days
**Dependencies:** None

**Tasks:**
1. Create folder structure and assembly definitions
2. Implement core data structures (VehicleData, PlayerShopData, enums)
3. Create VehicleCatalog ScriptableObject
4. Implement ServiceLocator
5. Set up basic scene hierarchy

**Deliverables:**
- All data classes compiling
- At least 3 test VehicleData assets
- Scene with basic canvas structure

**Verification:**
- ScriptableObjects create and serialize properly
- ServiceLocator registers and retrieves services

---

### Phase 2: Vehicle Display System (Complexity: M)
**Duration:** 2-3 days
**Dependencies:** Phase 1

**Tasks:**
1. Create VehicleShowcase component
2. Implement platform rotation
3. Create VehicleDisplayInstance for material control
4. Implement vehicle transition animations
5. Set up three-point lighting rig
6. Create placeholder vehicle prefabs

**Deliverables:**
- Working platform with rotation
- Vehicle can be spawned and colored
- Smooth transitions between vehicles

**Verification:**
- Vehicle rotates smoothly at 10 deg/sec
- Color changes apply immediately
- Transitions complete in 500ms

---

### Phase 3: Carousel System (Complexity: L)
**Duration:** 3-4 days
**Dependencies:** Phase 1

**Tasks:**
1. Create CarouselItem prefab with states (default, hover, selected, locked, owned)
2. Implement CarouselView with infinite wrapping
3. Create carousel navigation animations
4. Implement vehicle icon pooling
5. Add visual feedback for all states
6. Create navigation arrow buttons

**Deliverables:**
- Fully functional carousel with infinite scroll
- All visual states working
- Smooth 300ms transitions

**Verification:**
- Navigate through all vehicles without gaps
- Wrapping works in both directions
- Visual states match GDD specifications

---

### Phase 4: Stats Panel (Complexity: S)
**Duration:** 1-2 days
**Dependencies:** Phase 1

**Tasks:**
1. Create StatSlider component with animated fill
2. Implement StatsPanelView layout
3. Create ColorButton component
4. Implement color selection logic
5. Add stat animation on vehicle change

**Deliverables:**
- Stats panel displays all vehicle info
- Color buttons work and highlight selection
- Stats animate smoothly

**Verification:**
- Stats fill in 400ms with easing
- Color selection updates vehicle immediately
- Locked vehicles show "???" for stats

---

### Phase 5: Purchase System (Complexity: M)
**Duration:** 2-3 days
**Dependencies:** Phases 1, 3, 4

**Tasks:**
1. Implement CurrencyService
2. Implement TransactionService
3. Create PurchasePanelView
4. Create ConfirmationPopup
5. Wire up purchase flow
6. Add success/failure notifications

**Deliverables:**
- Complete purchase flow working
- Currency deduction working
- Owned vehicles tracked

**Verification:**
- Cannot purchase without funds
- Cannot purchase already-owned vehicles
- Balance updates immediately

---

### Phase 6: Input System (Complexity: M)
**Duration:** 2-3 days
**Dependencies:** Phases 3, 4, 5

**Tasks:**
1. Install and configure New Input System
2. Create ShopInputActions asset
3. Implement ShopInputHandler
4. Add hold-to-scroll functionality
5. Implement input debouncing
6. Add controller navigation

**Deliverables:**
- All input methods working (keyboard, mouse, controller)
- Hold navigation accelerates
- Input properly debounced

**Verification:**
- All controls from GDD work
- No double-inputs or missed inputs
- Controller focus navigation works

---

### Phase 7: Polish and Integration (Complexity: M)
**Duration:** 2-3 days
**Dependencies:** All previous phases

**Tasks:**
1. Add all UI animations per GDD specs
2. Integrate audio hooks
3. Add tutorial system hooks
4. Implement edge case handling
5. Performance optimization pass
6. Add accessibility features

**Deliverables:**
- All animations matching GDD timing
- Audio events firing correctly
- All edge cases handled

**Verification:**
- 60 FPS on PC, 30 FPS on mobile
- All edge cases from GDD covered
- Audio plays for all interactions

---

### Phase 8: Testing and Documentation (Complexity: S)
**Duration:** 1-2 days
**Dependencies:** Phase 7

**Tasks:**
1. Write unit tests for services
2. Create integration test scenes
3. Document public APIs
4. Create usage examples
5. Performance profiling

**Deliverables:**
- Test coverage for core logic
- API documentation
- Performance report

**Verification:**
- All tests pass
- Documentation complete
- Performance targets met

---

## 8. Resource Requirements

### 8.1 Assets Needed

| Asset Type | Count | Specification |
|------------|-------|---------------|
| Vehicle 3D Models | 6-10 | 3 LOD levels, UV mapped for color |
| Vehicle Icons | 6-10 | 256x256 PNG with alpha |
| Platform Model | 1 | Metallic circular, reflection-ready |
| UI Sprites | ~20 | Various buttons, frames, icons |
| Fonts | 2 | Racing-style header, clean body |
| Audio Clips | 10-15 | UI sounds per GDD |

### 8.2 Prefab Structure

```
Assets/CarBuy/Prefabs/
├── UI/
│   ├── ShopCanvas.prefab           # Main shop UI canvas
│   ├── CarouselItem.prefab         # Individual carousel vehicle item
│   ├── ColorButton.prefab          # Color selection button
│   ├── StatSlider.prefab           # Animated stat bar
│   └── ConfirmationPopup.prefab    # Purchase confirmation modal
├── Vehicle/
│   ├── VehicleShowcase.prefab      # Complete showcase setup
│   ├── DisplayPlatform.prefab      # Rotating platform
│   └── Vehicles/                   # Individual vehicle prefabs
│       ├── Vehicle_Placeholder.prefab
│       └── ...
└── Lighting/
    └── ShopLightingRig.prefab      # Three-point lighting setup
```

### 8.3 ScriptableObject Assets

```
Assets/CarBuy/Data/
├── Vehicles/
│   ├── Vehicle_NissanSkyline.asset
│   ├── Vehicle_FordMustang.asset
│   └── ...
├── VehicleCatalog.asset            # Master list of all vehicles
├── ShopSettings.asset              # Configurable shop parameters
└── ColorPalettes/
    └── DefaultColors.asset         # Shared color options
```

---

## 9. Performance Strategy

### 9.1 Target Metrics

| Platform | FPS | Draw Calls | Memory |
|----------|-----|------------|--------|
| PC | 60+ | < 500 | < 512 MB |
| Console | 60 | < 400 | < 512 MB |
| Mobile | 30+ | < 200 | < 256 MB |

### 9.2 Optimization Approaches

**Object Pooling:**
- Pool CarouselItem instances (create 9, reuse for infinite scroll)
- Pool notification UI elements
- Pool particle effects

**LOD System:**
- Vehicle models: 3 LOD levels
  - LOD0: Full detail (showcase)
  - LOD1: Medium (not used in shop)
  - LOD2: Low (carousel icons - if 3D)

**UI Optimization:**
- Use sprite atlases for UI elements
- Disable raycast targets on non-interactive elements
- Use CanvasGroup for batch fading
- Separate static and dynamic canvases

**Material Optimization:**
- Use MaterialPropertyBlock for color changes (no material instances)
- Share materials where possible
- GPU instancing for repeated elements

### 9.3 Profiling Points

| What to Measure | When | Target |
|-----------------|------|--------|
| Vehicle transition | Every vehicle change | < 16ms spike |
| Carousel scroll | During navigation | < 8ms per frame |
| Color change | On color select | < 2ms |
| Purchase flow | On confirm | < 50ms |
| Memory allocation | Continuous | No GC spikes |

---

## 10. Testing Approach

### 10.1 Unit Tests

**Services:**
```csharp
[Test] public void CurrencyService_DeductSucceeds_WhenSufficientFunds();
[Test] public void CurrencyService_DeductFails_WhenInsufficientFunds();
[Test] public void TransactionService_PurchaseSucceeds_WhenAffordableAndNotOwned();
[Test] public void TransactionService_PurchaseFails_WhenAlreadyOwned();
[Test] public void VehicleService_ReturnsCorrectVehicle_ById();
```

**Data:**
```csharp
[Test] public void PlayerShopData_OwnsVehicle_ReturnsTrueAfterPurchase();
[Test] public void VehicleData_StatsInValidRange();
```

### 10.2 Integration Tests

- Full purchase flow from selection to confirmation
- Carousel wrapping at boundaries
- Input system with all device types
- Save/Load cycle preserves purchases

### 10.3 Manual Testing Scenarios

| Scenario | Steps | Expected Result |
|----------|-------|-----------------|
| First-time user | Open shop with no owned vehicles | Tutorial prompts appear |
| Insufficient funds | Select expensive vehicle | Price red, button disabled |
| Rapid navigation | Hold direction for 3 seconds | Scrolling accelerates |
| Purchase cancellation | Confirm NO on popup | Return to browsing unchanged |
| Own all vehicles | Purchase last vehicle | Congratulations message |

### 10.4 Edge Cases

- Navigate to same vehicle from both directions
- Color change during vehicle transition
- Purchase button spam during processing
- Network disconnect during transaction (if applicable)
- Zero vehicles in catalog
- Single vehicle in catalog

---

## 11. Risk Mitigation

### 11.1 Technical Risks

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Vehicle model loading causes frame drops | Medium | High | Use async loading, show loading indicator |
| Material property block doesn't work on all shaders | Low | Medium | Provide fallback to material instances |
| Input system conflicts with other systems | Medium | Medium | Namespace all input actions, use input action asset |
| Memory leak from vehicle instances | Medium | High | Strict pooling discipline, destroy verification |
| UI scaling issues on extreme resolutions | Medium | Low | Test on multiple resolutions, use anchors properly |

### 11.2 Contingency Plans

**If vehicle transitions are too slow:**
- Pre-load adjacent vehicles
- Use simpler transition (crossfade only)
- Reduce model complexity

**If carousel performance degrades:**
- Reduce visible items
- Use static images instead of 3D previews
- Implement virtualized scrolling

**If purchase flow has race conditions:**
- Add state machine for purchase states
- Implement transaction queue
- Add idempotency keys

### 11.3 Early Warning Signs

- Frame time > 20ms during transitions
- Memory growth > 10MB per vehicle change
- Input lag > 100ms
- UI thread blocking > 16ms

---

## 12. Alternative Approaches

### Option A: MVP with Service Layer (Recommended)

**Pros:**
- Clear separation of concerns
- Highly testable
- Scalable for future features
- Follows Unity best practices

**Cons:**
- More initial setup
- More files to manage
- Slight learning curve for junior devs

### Option B: Monolithic MonoBehaviour Approach

**Pros:**
- Faster initial implementation
- All code in one place
- Familiar to beginners

**Cons:**
- Hard to test
- Hard to extend
- Will need refactoring for any new feature
- Tight coupling

### Option C: UI Toolkit Instead of uGUI

**Pros:**
- More modern
- Better styling capabilities
- Closer to web development patterns

**Cons:**
- Less mature in Unity 2022.3
- Less community resources
- Team may need training
- 3D integration more complex

### Decision Matrix

| Criteria | Weight | Option A | Option B | Option C |
|----------|--------|----------|----------|----------|
| Testability | 25% | 9 | 3 | 7 |
| Scalability | 25% | 9 | 4 | 8 |
| Development Speed | 20% | 6 | 9 | 5 |
| Team Familiarity | 15% | 7 | 9 | 4 |
| Future-proofing | 15% | 9 | 3 | 9 |
| **Weighted Score** | 100% | **7.9** | 5.25 | 6.65 |

**Recommendation:** Option A (MVP with Service Layer) provides the best balance of maintainability, testability, and scalability while remaining accessible to developers of varying skill levels.

---

## 13. File Structure Summary

```
/Users/muratkirdal/CarBuy/Assets/
└── CarBuy/
    ├── Scripts/
    │   ├── CarBuy.asmdef
    │   ├── Core/
    │   │   ├── ServiceLocator.cs
    │   │   ├── GameEvents.cs
    │   │   └── Extensions/
    │   │       └── TaskExtensions.cs
    │   ├── Data/
    │   │   ├── VehicleData.cs
    │   │   ├── VehicleStats.cs
    │   │   ├── VehicleColorOption.cs
    │   │   ├── VehicleClass.cs
    │   │   ├── VehicleCatalog.cs
    │   │   ├── VehicleModelInfo.cs
    │   │   ├── PlayerShopData.cs
    │   │   └── PurchaseTransaction.cs
    │   ├── Services/
    │   │   ├── Interfaces/
    │   │   │   ├── IVehicleService.cs
    │   │   │   ├── ICurrencyService.cs
    │   │   │   ├── ITransactionService.cs
    │   │   │   └── ISaveProvider.cs
    │   │   ├── VehicleService.cs
    │   │   ├── CurrencyService.cs
    │   │   ├── TransactionService.cs
    │   │   └── LocalSaveProvider.cs
    │   ├── UI/
    │   │   ├── ShopController.cs
    │   │   ├── ShopUIState.cs
    │   │   ├── Carousel/
    │   │   │   ├── CarouselView.cs
    │   │   │   └── CarouselItem.cs
    │   │   ├── Stats/
    │   │   │   ├── StatsPanelView.cs
    │   │   │   ├── StatSlider.cs
    │   │   │   └── ColorButton.cs
    │   │   ├── Purchase/
    │   │   │   └── PurchasePanelView.cs
    │   │   └── Common/
    │   │       ├── ConfirmationPopup.cs
    │   │       └── NotificationToast.cs
    │   ├── Vehicle/
    │   │   ├── VehicleShowcase.cs
    │   │   └── VehicleDisplayInstance.cs
    │   ├── Input/
    │   │   ├── ShopInputHandler.cs
    │   │   └── ShopInputActions.inputactions
    │   └── Audio/
    │       ├── IAudioService.cs
    │       └── ShopAudioEvents.cs
    ├── Prefabs/
    │   ├── UI/
    │   ├── Vehicle/
    │   └── Lighting/
    ├── Data/
    │   ├── Vehicles/
    │   ├── VehicleCatalog.asset
    │   └── ShopSettings.asset
    ├── Scenes/
    │   └── ShopScene.unity
    ├── Art/
    │   ├── UI/
    │   ├── Models/
    │   └── Materials/
    └── Audio/
        └── SFX/
```

---

## 14. Unresolved Questions

Before implementation begins, the following questions need clarification:

1. **Save System Integration:**
   - Will there be an existing save system to integrate with, or should we implement standalone persistence?
   - Is cloud save required for this phase?

2. **Currency Source:**
   - How does the player earn currency? (Needed for testing and balance display)
   - Is there a maximum currency cap?

3. **Vehicle Unlocking:**
   - What are the unlock conditions for locked vehicles?
   - Should unlock progress be visible in the shop?

4. **Audio Implementation:**
   - Will audio be handled by an existing audio manager?
   - Should we implement a standalone audio system for the shop?

5. **Garage Integration:**
   - What is the interface for the "View in Garage" button?
   - How does the player equip a purchased vehicle?

6. **Asset Pipeline:**
   - Who provides the 3D vehicle models?
   - What is the expected polycount range?
   - Are vehicle models already UV-mapped for color changes?

7. **Networking:**
   - Is this purely single-player, or are there multiplayer considerations?
   - Should transactions be validated server-side?

---

## 15. Next Steps

### Immediate Actions (Week 1)

1. **Environment Setup**
   - Install New Input System package
   - Install Cinemachine package (optional but recommended)
   - Consider installing DOTween for animations

2. **Project Structure**
   - Create folder hierarchy as specified
   - Create assembly definition files
   - Set up basic scene with Canvas

3. **Data Foundation**
   - Implement all data classes (VehicleData, PlayerShopData, etc.)
   - Create VehicleCatalog ScriptableObject
   - Create 3 test vehicle assets with placeholder data

4. **Placeholder Assets**
   - Create simple cube/capsule vehicle prefabs
   - Create basic UI sprites
   - Set up placeholder materials with color property

5. **Prototype Vehicle Display**
   - Implement basic VehicleShowcase with rotation
   - Test material color changing
   - Verify three-point lighting setup

### Success Milestone (End of Week 1)
- Can spawn a placeholder vehicle on rotating platform
- Can change vehicle color via code
- Data classes serialize correctly in Inspector

---

**Document prepared for implementation of the Car Purchase System as specified in the GDD.**

