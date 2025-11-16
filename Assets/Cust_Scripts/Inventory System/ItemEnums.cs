using UnityEngine;

public static class ItemEnums
{
    // this file will hold all enums related to items in the inventory system

    public enum ItemType // types of items
    {
        Consumable,
        Equipment,
        QuestItem,
        Miscellaneous
    }

    public enum EquipmentType // types of equipment
    {
        Head,
        Chest,
        Legs,
        Weapon,
        Shield,
        Accessory
    }

    public enum Rarity // item rarity levels
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum WeaponType // types of weapons
    {
        Melee,
        Ranged,
        Magic
    }

    public enum ConsumableType // types of consumables
    {
        HealthPotion,
        ManaPotion,
        StaminaPotion,
        Food,
        Buff
    }

    public enum QuestItemType // types of quest items
    {
        KeyItem,
        Collectible,
        StoryItem
    }
    public enum MiscellaneousType // types of miscellaneous items
    {
        CraftingMaterial,
        Currency,
        Decoration
    }

    public enum ToolType // types of tools
    {
        Pickaxe,
        Axe,
        Shovel,
        FishingRod
    }

    // Add more enums as needed for the inventory system
}
