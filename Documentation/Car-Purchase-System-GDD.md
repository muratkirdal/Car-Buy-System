# Car Purchase System - Game Design Document

## Document Information

| Field | Value |
|-------|-------|
| **Document Title** | Car Purchase System GDD |
| **Version** | 1.0 |
| **Date** | January 28, 2026 |
| **Inspiration** | Need for Speed Underground 2 |
| **System Type** | Vehicle Acquisition Interface |

---

## 1. Introduction

### 1.1 Executive Summary

This document defines the design specifications for a Car Purchase System inspired by the iconic vehicle selection interface from Need for Speed Underground 2. The system provides players with an immersive, visually engaging method to browse, customize, and purchase vehicles for their garage.

### 1.2 Design Goals

| Priority | Goal | Success Criteria |
|----------|------|------------------|
| Primary | **Intuitive Navigation** | Players understand controls within 5 seconds |
| Primary | **Visual Appeal** | Vehicle presentation creates desire to purchase |
| Primary | **Clear Information** | Stats are immediately readable and comparable |
| Secondary | **Satisfying Feedback** | Every interaction provides audio/visual response |
| Secondary | **Efficient Flow** | Purchase completed in under 30 seconds |

### 1.3 Player Fantasy

"I am browsing an exclusive underground car dealership, examining powerful machines, customizing their appearance, and adding my dream car to my collection."

### 1.4 Target Platform

- Primary: PC, Console (PlayStation, Xbox)
- Secondary: Mobile (adapted layout)
- Input Methods: Controller, Keyboard/Mouse, Touch

---

## 2. System Overview

### 2.1 Screen Layout

```
+------------------------------------------------------------------+
|                                              [PURCHASE PANEL]     |
|                                              +-----------------+  |
|  [STATS PANEL]                               | Price: $45,000  |  |
|  +----------------+                          | [PURCHASE BTN]  |  |
|  | NISSAN SKYLINE |                          +-----------------+  |
|  |----------------|                                               |
|  | Speed:    [====------]                                         |
|  | Accel:    [=======---]                                         |
|  | Handling: [=====-----]           +-------------------+         |
|  |----------------|                 |                   |         |
|  | Colors:        |                 |   [ROTATING CAR]  |         |
|  | [R][B][W][K][S][G]               |    ON PLATFORM    |         |
|  +----------------+                 |                   |         |
|                                     +-------------------+         |
|                                                                   |
|------------------------------------------------------------------+
|    [<]  [Car1] [Car2] [Car3*] [Car4] [Car5] [Car6]  [>]          |
+------------------------------------------------------------------+
```

### 2.2 Component Breakdown

| Component | Location | Purpose |
|-----------|----------|---------|
| Vehicle Carousel | Bottom | Browse available vehicles |
| Vehicle Display | Center | Showcase selected vehicle in 3D |
| Stats Panel | Left | Display performance metrics and color options |
| Purchase Panel | Top Right | Show price and initiate purchase |
| Confirmation Popup | Center (overlay) | Confirm purchase decision |

---

## 3. Core Mechanics and Interactions

### 3.1 Vehicle Selection Carousel

#### 3.1.1 Layout Specifications

- **Position**: Fixed to bottom edge of screen
- **Height**: 15% of screen height
- **Visible Vehicles**: 5-7 icons depending on screen width
- **Selected Vehicle**: Centered, visually highlighted

#### 3.1.2 Navigation Controls

| Input Device | Left Navigation | Right Navigation | Quick Select |
|--------------|-----------------|------------------|--------------|
| Controller | D-Pad Left / Left Stick | D-Pad Right / Right Stick | A/X Button on icon |
| Keyboard | A / Left Arrow | D / Right Arrow | Enter on highlighted |
| Mouse | Click Left Arrow | Click Right Arrow | Click vehicle icon |
| Touch | Tap Left Arrow / Swipe Right | Tap Right Arrow / Swipe Left | Tap vehicle icon |

#### 3.1.3 Visual Feedback

| State | Visual Treatment |
|-------|------------------|
| Default | Vehicle icon at 80% opacity, standard size |
| Hovered | Icon at 100% opacity, subtle glow effect |
| Selected | Icon at 100% opacity, scale 1.2x, golden border, elevated position |
| Locked | Grayscale, lock icon overlay, 60% opacity |
| Owned | Checkmark badge in corner |

#### 3.1.4 Carousel Behavior

- **Wrapping**: Carousel wraps infinitely (last vehicle connects to first)
- **Animation Duration**: 0.3 seconds per vehicle transition
- **Easing**: Ease-out cubic for smooth deceleration
- **Hold Navigation**: Holding direction key accelerates scroll after 0.5 seconds

### 3.2 Vehicle Display Platform

#### 3.2.1 Presentation

- **Platform Type**: Circular metallic platform with subtle reflections
- **Platform Animation**: Continuous slow rotation (10 degrees per second clockwise)
- **Vehicle Entry**: New vehicle drives/slides onto platform from right when selected
- **Lighting**: Three-point lighting setup with dramatic key light

#### 3.2.2 Camera Behavior

| Interaction | Camera Response |
|-------------|-----------------|
| Idle | Fixed position, vehicle rotates on platform |
| Vehicle Change | Brief zoom out during transition, zoom back in |
| Color Change | Quick flash/pulse effect on vehicle |
| Purchase Confirm | Camera pushes in dramatically |

#### 3.2.3 Vehicle Transition Animation

1. Current vehicle fades out (0.2 seconds)
2. Platform rotation pauses
3. New vehicle fades in from selected direction (0.3 seconds)
4. Platform rotation resumes
5. Total transition time: 0.5 seconds

### 3.3 Stats Panel

#### 3.3.1 Panel Layout

```
+------------------------+
| [VEHICLE NAME]         |  <- Header (vehicle name, class icon)
|------------------------|
| PERFORMANCE            |  <- Section label
|                        |
| Speed      [=====----] |  <- Stat slider 1
|            78 / 100    |  <- Numeric value
|                        |
| Acceleration [===----] |  <- Stat slider 2
|              62 / 100  |
|                        |
| Handling   [======---] |  <- Stat slider 3
|            85 / 100    |
|------------------------|
| COLORS                 |  <- Section label
| [O] [O] [O] [O] [O] [O]|  <- 6 color buttons
+------------------------+
```

#### 3.3.2 Stat Slider Specifications

| Property | Value |
|----------|-------|
| Minimum Value | 0 |
| Maximum Value | 100 |
| Slider Width | 200 pixels (scalable) |
| Fill Color | Gradient from yellow to green based on value |
| Empty Color | Dark gray (#333333) |
| Animation | Fill animates when vehicle changes (0.4 seconds) |

#### 3.3.3 Stat Definitions

| Stat | Description | Affects |
|------|-------------|---------|
| **Speed** | Maximum velocity the vehicle can achieve | Top speed in races, highway performance |
| **Acceleration** | How quickly the vehicle reaches top speed | Launch speed, recovery from corners |
| **Handling** | Responsiveness and grip during turns | Cornering ability, drift control |

#### 3.3.4 Color Selection System

| Element | Specification |
|---------|---------------|
| Button Count | 6 color options per vehicle |
| Button Shape | Circular, 40x40 pixels |
| Button Spacing | 8 pixels between buttons |
| Selected State | White border (3px), slight scale up (1.1x) |
| Default State | No border, standard scale |
| Hover State | Subtle glow matching button color |

**Color Application Behavior:**
- Color change is instant on click/selection
- Vehicle material updates in real-time
- Brief particle effect or shine animation plays on vehicle
- Color selection persists until changed or vehicle switched

### 3.4 Purchase Panel

#### 3.4.1 Panel Layout

```
+----------------------+
| NISSAN SKYLINE GT-R  |  <- Vehicle name
|----------------------|
|     $45,000          |  <- Price display
|                      |
|   [  PURCHASE  ]     |  <- Purchase button
|                      |
| Your Balance: $78,500|  <- Player's current currency
+----------------------+
```

#### 3.4.2 Price Display

| Scenario | Display Format |
|----------|----------------|
| Standard | "$XX,XXX" in large, prominent font |
| On Sale | Original price struck through, sale price in green |
| Cannot Afford | Price displayed in red, purchase button disabled |
| Already Owned | "OWNED" badge replaces price |

#### 3.4.3 Purchase Button States

| State | Visual | Behavior |
|-------|--------|----------|
| Available | Green/Gold, fully lit | Clickable, opens confirmation |
| Insufficient Funds | Grayed out, red tint | Not clickable, tooltip shows "Insufficient funds" |
| Already Owned | Hidden or shows "VIEW IN GARAGE" | Redirects to garage |
| Processing | Loading spinner | Not clickable during transaction |

### 3.5 Confirmation Popup

#### 3.5.1 Popup Layout

```
+----------------------------------+
|        CONFIRM PURCHASE          |
|----------------------------------|
|                                  |
|    Purchase NISSAN SKYLINE       |
|    for $45,000?                  |
|                                  |
|    [  YES  ]      [  NO  ]       |
|                                  |
+----------------------------------+
```

#### 3.5.2 Popup Behavior

| Property | Value |
|----------|-------|
| Appearance | Fade in over 0.2 seconds with slight scale up |
| Background | Darkened overlay (50% black) behind popup |
| Position | Center of screen |
| Default Focus | "No" button (prevents accidental purchases) |
| Dismiss Methods | Click No, Press Escape/B, Click outside popup |

#### 3.5.3 Confirmation Actions

**YES Button:**
1. Popup closes with fade out
2. Purchase processing animation plays
3. Currency deducted from player balance
4. Success notification appears
5. Vehicle marked as owned in carousel
6. Optional: Prompt to equip vehicle immediately

**NO Button:**
1. Popup closes with fade out
2. Return to normal shopping state
3. No changes to data

---

## 4. User Flow and Navigation

### 4.1 Primary User Flow

```
[ENTER SHOP]
     |
     v
[DEFAULT VIEW: First vehicle selected]
     |
     +---> [BROWSE VEHICLES] <---+
     |            |              |
     |            v              |
     |     [SELECT VEHICLE]      |
     |            |              |
     |            v              |
     |     [VIEW STATS]          |
     |            |              |
     |            +---> [CHANGE COLOR] ---> [COLOR APPLIED] ---+
     |            |                                            |
     |            v                                            |
     |     [CLICK PURCHASE]                                    |
     |            |                                            |
     |            v                                            |
     |     [CONFIRMATION POPUP]                                |
     |       /          \                                      |
     |      v            v                                     |
     | [YES]          [NO] ---------------------------------->+
     |    |
     |    v
     | [PURCHASE SUCCESS]
     |    |
     |    v
     | [VEHICLE ADDED TO GARAGE]
     |    |
     |    +---> [CONTINUE SHOPPING] or [EXIT TO GARAGE]
```

### 4.2 Navigation Map

| From | Action | To |
|------|--------|-----|
| Any State | Press Back/Escape | Previous Screen or Exit Shop |
| Vehicle Selected | Press Left/Right | Adjacent Vehicle |
| Vehicle Selected | Click Color Button | Color Applied to Vehicle |
| Vehicle Selected | Click Purchase | Confirmation Popup |
| Confirmation Popup | Click Yes | Purchase Processing |
| Confirmation Popup | Click No | Vehicle Selected (unchanged) |
| Purchase Success | Click Continue | Vehicle Selected (browsing) |
| Purchase Success | Click Exit | Garage Screen |

### 4.3 Keyboard/Controller Shortcut Map

| Action | Keyboard | Controller |
|--------|----------|------------|
| Navigate Left | A / Left Arrow | D-Pad Left / Left Stick Left |
| Navigate Right | D / Right Arrow | D-Pad Right / Right Stick Right |
| Select/Confirm | Enter / Space | A (Xbox) / X (PlayStation) |
| Cancel/Back | Escape | B (Xbox) / O (PlayStation) |
| Quick Purchase | P | Y (Xbox) / Triangle (PlayStation) |
| Cycle Colors | Q / E | LB/RB (Xbox) / L1/R1 (PlayStation) |
| Exit Shop | Escape (hold) | Start / Options |

---

## 5. UI/UX Specifications

### 5.1 Visual Style Guide

| Element | Specification |
|---------|---------------|
| **Overall Aesthetic** | Underground street racing, neon accents, dark backgrounds |
| **Primary Colors** | Deep black (#0D0D0D), Electric blue (#00D4FF), Gold (#FFD700) |
| **Accent Colors** | Neon pink (#FF0080), Neon green (#39FF14) |
| **Font Family** | Primary: "Rajdhani" or similar angular, racing-style font |
| **Font Sizes** | Headers: 32px, Body: 18px, Labels: 14px |

### 5.2 Panel Styling

| Panel | Background | Border | Shadow |
|-------|------------|--------|--------|
| Stats Panel | Semi-transparent black (85% opacity) | 1px cyan glow | Soft drop shadow |
| Purchase Panel | Semi-transparent black (85% opacity) | 1px gold glow | Soft drop shadow |
| Carousel | Gradient fade from transparent to black | None | None |
| Confirmation | Solid dark gray (#1A1A1A) | 2px white | Heavy drop shadow |

### 5.3 Animation Specifications

| Animation | Duration | Easing | Trigger |
|-----------|----------|--------|---------|
| Vehicle Transition | 500ms | Ease-out | Vehicle selection change |
| Stat Bar Fill | 400ms | Ease-in-out | Vehicle selection change |
| Color Change Flash | 200ms | Linear | Color button click |
| Button Hover | 150ms | Ease-out | Mouse enter |
| Popup Appear | 200ms | Ease-out | Purchase click |
| Popup Dismiss | 150ms | Ease-in | Cancel/Confirm |
| Purchase Success | 800ms | Custom bounce | Transaction complete |

### 5.4 Audio Design

| Event | Sound Type | Description |
|-------|------------|-------------|
| Vehicle Select | UI Click | Mechanical click with slight engine rev undertone |
| Arrow Button Press | UI Click | Soft directional whoosh |
| Color Change | UI Confirm | Spray paint burst sound |
| Purchase Button Hover | UI Hover | Subtle electronic hum |
| Purchase Button Click | UI Action | Cash register with engine start |
| Popup Appear | UI Modal | Dramatic low-frequency impact |
| Confirm Purchase | UI Success | Celebratory jingle with crowd cheer |
| Cancel Purchase | UI Cancel | Soft decline tone |
| Insufficient Funds | UI Error | Error buzz with cash declining sound |

### 5.5 Responsive Design Considerations

| Screen Size | Adaptation |
|-------------|------------|
| 4K (3840x2160) | Full layout, increased detail on vehicle model |
| 1080p (1920x1080) | Standard layout |
| 720p (1280x720) | Slightly condensed panels, smaller fonts |
| Mobile Portrait | Stacked layout: Display top, Stats middle, Carousel bottom |
| Mobile Landscape | Similar to console, reduced panel sizes |

---

## 6. Data Structure Considerations

### 6.1 Vehicle Data Model

```
Vehicle {
    id: string (unique identifier)
    name: string (display name)
    manufacturer: string
    class: enum (Compact, Sports, Muscle, Exotic, Tuner)
    price: integer (in-game currency)
    isLocked: boolean (requires unlock condition)
    unlockCondition: string (description if locked)

    stats: {
        speed: integer (0-100)
        acceleration: integer (0-100)
        handling: integer (0-100)
    }

    colors: [
        {
            id: string
            name: string
            hexCode: string
            materialType: enum (Matte, Metallic, Pearlescent)
        }
    ] // Array of 6 color options

    model3D: {
        assetPath: string
        previewIconPath: string
        scale: float
        rotationOffset: float
    }
}
```

### 6.2 Player Data Model

```
PlayerShopData {
    playerId: string
    balance: integer (current currency)

    ownedVehicles: [
        {
            vehicleId: string
            purchaseDate: timestamp
            selectedColor: string (color id)
        }
    ]

    currentSelection: {
        vehicleId: string
        previewColor: string
    }
}
```

### 6.3 Transaction Data Model

```
PurchaseTransaction {
    transactionId: string
    playerId: string
    vehicleId: string
    selectedColor: string
    price: integer
    timestamp: timestamp
    status: enum (Pending, Completed, Failed, Refunded)
}
```

### 6.4 UI State Model

```
ShopUIState {
    currentVehicleIndex: integer
    carouselOffset: integer (for smooth scrolling)
    selectedColorIndex: integer
    isPurchasePopupOpen: boolean
    isProcessingPurchase: boolean
    errorMessage: string (nullable)
}
```

---

## 7. Edge Cases and Special Scenarios

### 7.1 Insufficient Funds Handling

| Scenario | System Response |
|----------|-----------------|
| Player selects unaffordable vehicle | Purchase button disabled, price shown in red |
| Player clicks disabled purchase button | Tooltip: "You need $X more to purchase this vehicle" |
| Player's balance updates while browsing | UI updates in real-time, button states refresh |

### 7.2 Already Owned Vehicles

| Scenario | System Response |
|----------|-----------------|
| Player selects owned vehicle | "OWNED" badge displayed, purchase button becomes "VIEW IN GARAGE" |
| Color change on owned vehicle | Color saved immediately to player data |
| Navigation includes owned vehicles | Owned vehicles remain in carousel with checkmark badge |

### 7.3 Locked Vehicles

| Scenario | System Response |
|----------|-----------------|
| Locked vehicle in carousel | Grayscale icon with lock overlay |
| Player selects locked vehicle | Vehicle displayed but grayed out, unlock requirements shown |
| Stats panel for locked vehicle | Stats hidden or shown as "???" |
| Purchase button for locked vehicle | Shows "UNLOCK REQUIREMENTS" button instead |

### 7.4 Network and Transaction Issues

| Scenario | System Response |
|----------|-----------------|
| Network disconnection during browse | Cached data used, "Offline Mode" indicator shown |
| Transaction fails | Error popup: "Purchase failed. Please try again." Refund any deducted currency |
| Duplicate purchase attempt | Prevent transaction, show "Vehicle already owned" |
| Server timeout | Retry automatically (3 attempts), then show error with retry button |

### 7.5 Rapid Input Handling

| Scenario | System Response |
|----------|-----------------|
| Rapid left/right navigation | Debounce inputs (100ms minimum between transitions) |
| Double-click on purchase | Ignore second click during popup animation |
| Click during vehicle transition | Queue action, execute after transition completes |
| Spam color buttons | Apply only most recent color, ignore intermediate |

### 7.6 Empty States

| Scenario | System Response |
|----------|-----------------|
| No vehicles available | Display message: "No vehicles available. Check back later!" |
| All vehicles owned | Display congratulations message, option to revisit owned vehicles |
| Zero balance | All prices shown in red, helpful tip: "Earn money in races!" |

### 7.7 First-Time User Experience

| Step | System Behavior |
|------|-----------------|
| First shop visit | Brief tutorial overlay highlighting each UI element |
| First vehicle hover | Tooltip: "Use arrows or click to browse vehicles" |
| First stats view | Tooltip: "Compare stats to find your perfect ride" |
| First color click | Tooltip: "Click colors to preview different looks" |
| First purchase attempt | Tooltip: "Ready to buy? Click Purchase to add to your garage" |

---

## 8. Technical Requirements

### 8.1 Performance Targets

| Metric | Target | Minimum Acceptable |
|--------|--------|-------------------|
| Frame Rate | 60 FPS | 30 FPS |
| Vehicle Load Time | < 500ms | < 1500ms |
| UI Response Time | < 100ms | < 200ms |
| Memory Footprint | < 512 MB | < 1 GB |

### 8.2 Asset Requirements

| Asset Type | Specification |
|------------|---------------|
| Vehicle 3D Models | LOD system with 3 levels (High/Medium/Low) |
| Vehicle Icons | 256x256 PNG with transparency |
| UI Elements | Vector-based (SVG) for scaling |
| Textures | 2K resolution for display vehicle, 512 for icons |

### 8.3 Save System

| Data | Save Trigger | Storage Location |
|------|--------------|------------------|
| Purchase History | Immediately on confirmed purchase | Server (with local backup) |
| Color Selection | On color change for owned vehicles | Server (with local backup) |
| Browse State | Not saved (resets on exit) | Memory only |
| Tutorial Progress | On tutorial step completion | Local storage |

### 8.4 Platform-Specific Considerations

| Platform | Consideration |
|----------|---------------|
| PC | Support for ultrawide monitors, multiple input methods |
| PlayStation | DualSense haptic feedback for selections, adaptive triggers for purchase |
| Xbox | Achievement integration for first purchase, collection milestones |
| Nintendo Switch | Touch screen support in handheld, reduced LOD for performance |
| Mobile | Battery optimization, data usage warnings for 3D model downloads |

---

## 9. Future Expansion Considerations

### 9.1 Potential Feature Additions

| Feature | Description | Priority |
|---------|-------------|----------|
| Vehicle Comparison | Side-by-side stat comparison of two vehicles | High |
| Test Drive | Brief test drive before purchase | High |
| Financing System | Pay in installments for expensive vehicles | Medium |
| Trade-In System | Trade owned vehicles for credit toward new ones | Medium |
| Wish List | Save vehicles for later | Low |
| Gift System | Purchase vehicles for friends | Low |

### 9.2 Additional Customization Options

| Option | Description |
|--------|-------------|
| Wheel Selection | Preview different wheel styles |
| Window Tint | Adjust window tint levels |
| Vinyl Preview | Preview popular vinyl designs |
| Performance Parts | Show potential stat increases with upgrades |

---

## 10. Appendix

### 10.1 Glossary

| Term | Definition |
|------|------------|
| Carousel | Horizontal scrolling list of vehicle icons at screen bottom |
| Platform | The rotating display surface where the selected vehicle sits |
| Stats Panel | Left-side UI element showing vehicle performance metrics |
| Confirmation Popup | Modal dialog requesting purchase confirmation |
| LOD | Level of Detail - varying 3D model complexity for performance |

### 10.2 Reference Games

| Game | Relevant Feature |
|------|------------------|
| Need for Speed Underground 2 | Overall aesthetic, vehicle presentation |
| Gran Turismo 7 | Stat presentation, dealership atmosphere |
| Forza Horizon 5 | Carousel navigation, vehicle transitions |
| CSR Racing 2 | Mobile-friendly vehicle selection |

### 10.3 Revision History

| Version | Date | Changes | Author |
|---------|------|---------|--------|
| 1.0 | 2026-01-28 | Initial document creation | GDD Creator |

---

## Next Steps

This Game Design Document provides a comprehensive foundation for the Car Purchase System. Recommended next actions:

1. **UI/UX Prototyping**: Create wireframes and interactive mockups to test flow
2. **Technical Design**: Develop Technical Design Document for implementation specifics
3. **Asset List**: Generate detailed asset requirements for art team
4. **User Testing**: Plan usability testing sessions for navigation validation
5. **Localization**: Identify all text strings requiring translation
