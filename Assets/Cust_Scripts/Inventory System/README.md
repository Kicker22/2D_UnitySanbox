# Inventory System Documentation

## Overview
This is a Unity-based inventory system designed for RPG survival games. It features a slot-based inventory with consumable items, stacking mechanics, and pause functionality.

## System Architecture

### Core Components

#### 1. **InventoryManager.cs** - Main Controller
**Location**: `Assets/Cust_Scripts/Inventory System/InventoryManager.cs`

**Purpose**: Central hub that manages inventory operations, UI toggling, and game pause functionality.

**Key Features**:
- **Input Handling**: Uses Unity's new Input System to toggle inventory with Tab key
- **Pause System**: Automatically pauses/resumes game when inventory opens/closes
- **Item Management**: Handles adding items and using consumables

**Important Methods**:
```csharp
ToggleInventory()        // Opens/closes inventory UI and handles pause
AddItem()               // Adds items to inventory slots with stacking
UseItem()              // Triggers consumable item effects
DeselectAllSlots()     // Clears all slot selections
```

**Inspector Setup**:
- `InventoryMenu`: GameObject containing the UI
- `toggleInventoryAction`: Input Action Reference for Tab key
- `pauseGameWhenOpen`: Toggle pause functionality
- `itemSlot[]`: Array of ItemSlot components
- `itemSos[]`: Array of ItemSo ScriptableObjects

---

#### 2. **ItemSlot.cs** - Individual Slot Logic
**Location**: `Assets/Cust_Scripts/Inventory System/ItemSlot.cs`

**Purpose**: Manages individual inventory slots, handles item stacking, and UI interactions.

**Key Features**:
- **Item Storage**: Holds item data (name, quantity, sprite, description)
- **Stack Management**: Handles item stacking with configurable max stack size
- **UI Integration**: Updates visual elements (quantity text, item icons)
- **Click Handling**: Left-click selects/uses items, right-click for future features

**Important Fields**:
```csharp
maxNumberOfItems      // Maximum stack size per slot
itemName             // Current item name
quantity             // Current stack quantity
isFull               // Whether slot has reached max capacity
```

**Stack Logic**:
- Items of the same type stack together
- When adding items exceeds `maxNumberOfItems`, overflow goes to next available slot
- Returns leftover quantity for recursive stacking

---

#### 3. **ItemSo.cs** - Item Data & Effects
**Location**: `Assets/Scriptable_Objects/ItemSo.cs`

**Purpose**: ScriptableObject that defines item properties and consumable effects.

**Key Features**:
- **Item Definition**: Name and effect properties
- **Consumable System**: Currently supports health restoration
- **Extensible**: Enum-based system for adding new stat types

**Consumable Types**:
```csharp
StatToChange.health   // Heals player
StatToChange.stamina  // Future: Restore stamina
StatToChange.mana     // Future: Restore mana
```

**Usage Flow**:
1. Player clicks on item slot
2. `ItemSlot` calls `InventoryManager.UseItem()`
3. `InventoryManager` finds matching `ItemSo`
4. `ItemSo.UseItem()` triggers the effect

---

#### 4. **PlayerHealth.cs** - Health System Integration
**Location**: `Assets/Cust_Scripts/player_health_system/PlayerHealth.cs`

**Purpose**: Manages player health and integrates with consumable items.

**Integration Points**:
- `Heal()` method called by consumable items
- Event system for UI updates: `OnHealthChanged`
- Built-in testing with H/J keys

## How the System Works

### Adding Items to Inventory
```
Item Pickup → InventoryManager.AddItem() → ItemSlot.AddItem() → UI Update
```

1. **External system** calls `InventoryManager.AddItem()` with item data
2. **InventoryManager** finds appropriate slot (existing stack or empty slot)
3. **ItemSlot** handles stacking logic and overflow
4. **UI elements** update automatically (quantity text, item icon)

### Using Consumable Items
```
Click Slot → ItemSlot.OnLeftClick() → InventoryManager.UseItem() → ItemSo.UseItem() → PlayerHealth.Heal()
```

1. **Player clicks** on inventory slot
2. **ItemSlot** detects if already selected (double-click to use)
3. **InventoryManager** finds matching ScriptableObject
4. **ItemSo** determines effect type and calls appropriate system
5. **PlayerHealth** applies the healing effect

### Inventory Toggle System
```
Tab Press → Input System → InventoryManager.ToggleInventory() → UI + Pause
```

1. **Input System** detects Tab key press
2. **InventoryManager** toggles UI visibility
3. **Game pauses/resumes** based on `pauseGameWhenOpen` setting
4. **Time.timeScale** controls game pause state

## Setup Instructions

### 1. **Scene Setup**
- Create InventoryCanvas GameObject
- Attach InventoryManager script
- Create UI panel for inventory (assign to InventoryMenu field)
- Create ItemSlot GameObjects with ItemSlot scripts

### 2. **Input System Setup**
- Create Input Actions asset
- Add UI Action Map with "Tab" action
- Assign to `toggleInventoryAction` in InventoryManager

### 3. **ScriptableObject Creation**
- Right-click in Project → Create → ItemSo
- Set `itemName`, `statToChange`, and `amountToChange`
- Add to `itemSos[]` array in InventoryManager

### 4. **ItemSlot Configuration**
- Assign UI references (quantityText, itemImage, etc.)
- Set `maxNumberOfItems` for stack size
- Connect description UI elements

## Common Issues & Solutions

### Issue: Items not stacking properly
**Cause**: Array index mismatch between `itemSlot.Length` and `itemSos.Length`
**Solution**: Ensure both arrays are same size, or use proper bounds checking

### Issue: Consumables not working
**Cause**: 
- `amountToChange` not set in ScriptableObject Inspector
- PlayerHealth GameObject not found
- ScriptableObject not assigned to InventoryManager

**Solution**: 
- Check Inspector values on ScriptableObjects
- Ensure PlayerHealth component exists in scene
- Verify `itemSos[]` array is populated

### Issue: Inventory not opening
**Cause**: Input Action not properly assigned or enabled
**Solution**: 
- Check `toggleInventoryAction` assignment
- Verify Input Actions asset is enabled
- Confirm Tab key binding in Input Actions

## Future Enhancements

### Planned Features
- **Multiple Item Categories**: Weapons, Armor, Tools, Materials
- **Durability System**: Items degrade with use
- **Drag & Drop**: Visual item movement between slots
- **Item Tooltips**: Detailed item information on hover
- **Save/Load System**: Persistent inventory state

### Extension Points
- Add new `StatToChange` enum values for different consumables
- Extend `ItemSo` with equipment stats (damage, armor, etc.)
- Add item rarity system with color coding
- Implement crafting system integration

## File Structure
```
Assets/
├── Cust_Scripts/
│   ├── Inventory System/
│   │   ├── InventoryManager.cs
│   │   └── ItemSlot.cs
│   └── player_health_system/
│       └── PlayerHealth.cs
└── Scriptable_Objects/
    └── ItemSo.cs
```

## Debug Tips

### Useful Debug Messages
- Enable debug logs in InventoryManager to trace item flow
- Check Console for "PlayerHealth component not found" errors
- Verify `itemSos` array indices match actual ScriptableObjects

### Testing Shortcuts
- Use H/J keys in PlayerHealth for manual heal/damage testing
- Create test items with different stack sizes
- Test inventory toggle with Tab key

---

*Last Updated: November 12, 2025*
*System Version: 1.0 - Basic Inventory with Consumables*