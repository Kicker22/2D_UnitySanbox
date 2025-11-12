# Health System Documentation

## Files Overview

### 1. PlayerHealth.cs
**Purpose**: Core health management system
**Key Features**:
- Health tracking with configurable max health
- Public properties for easy access (CurrentHealth, MaxHealth, HealthRatio, IsAlive)
- Events for health changes and death
- Damage and healing methods with validation
- Dev cheats (editor-only) for testing

**Public API**:
- `TakeDamage(int amount)` - Apply damage
- `Heal(int amount)` - Restore health
- `FullHeal()` - Restore to max health
- `SetMaxHealth(int newMax)` - Change max health while preserving ratio

### 2. PlayerControl.cs
**Purpose**: Main player controller that coordinates health events
**Key Features**:
- Forwards health events to UI systems
- Handles low health warnings
- Manages death state
- Proper event subscription/unsubscription

**Events**:
- `OnHealthChanged(int current, int max)` - Health value changed
- `OnPlayerDeath()` - Player died
- `OnLowHealth()` - Health below threshold

### 3. GameUIHandler.cs
**Purpose**: UI Toolkit-based health display
**Key Features**:
- Updates health label and bar visuals
- Color-coded health bar (green/red)
- Null reference protection
- Proper event cleanup
- Configurable health bar appearance

**Required UI Elements**:
- "HealthLabel" (Label)
- "HealthBarMask" (VisualElement)
- "HealthBarFill" (VisualElement) - optional for color changes

### 4. HealthBarUI.cs
**Purpose**: Legacy Canvas UI health bar (alternative to GameUIHandler)
**Key Features**:
- Traditional Unity UI (Canvas) support
- Animated health bar transitions
- Slider-based health visualization
- Color transitions based on health level

## Usage Instructions

### Basic Setup:
1. Add `PlayerHealth` component to player GameObject
2. Add `PlayerControl` component to same GameObject
3. Create UI using either:
   - **UI Toolkit**: Use `GameUIHandler` with UIDocument
   - **Canvas UI**: Use `HealthBarUI` with Canvas/Slider components

### Event Flow:
```
PlayerHealth → PlayerControl → UI Systems
     ↓              ↓              ↓
  Core Logic   Coordination   Display/Effects
```

### Dev Cheats (Editor Only):
- **H Key**: Heal 10 HP
- **J Key**: Take 10 damage
- **F Key**: Full heal

## Bug Fixes Applied:
1. ✅ Fixed missing UI element references
2. ✅ Added proper event unsubscription (memory leak prevention)
3. ✅ Added null reference checks
4. ✅ Fixed deprecated Unity API calls
5. ✅ Added validation for health operations
6. ✅ Proper separation of concerns between components
7. ✅ Added comprehensive error handling
8. ✅ Made dev cheats editor-only with conditional compilation

## Configuration Options:
- Max Health (PlayerHealth)
- Low Health Threshold (PlayerControl & UI)
- Health Bar Visual Range (GameUIHandler)
- Health Bar Colors (Both UI systems)
- Animation Speed (HealthBarUI)