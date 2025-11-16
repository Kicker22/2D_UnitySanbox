using UnityEngine;

// This class represents a slot in the inventory system
public class InventorySlot : MonoBehaviour
{
    public Item storedItem; // The item stored in this inventory slot
    public int quantity; // Quantity of the item in this slot
    public int slotIndex; // Index of the slot in the inventory

    // Method to add an item to the inventory slot 
    public void AddItem(Item item, int qty, float durability = 100f)
    {
        storedItem = item; // Assign the item to the slot
        quantity = qty; // Set the quantity

        // Additional logic for updating UI or other systems can be added here
    }

    // Method to clear the slot
    public void ClearSlot()
    {
        storedItem = null;
        quantity = 0;
    }

    // Method to check if the slot is empty
    public bool IsEmpty()
    {
        return storedItem == null;
    }

    // Method to get item information
    public string GetItemInfo()
    {
        if (IsEmpty())
        {
            return "Empty Slot";
        }
        else
        {
            return $"{storedItem.itemName} x{quantity}";
        }
    }

    // Method to use the item in the slot
    public void UseItem()
    {
        if (!IsEmpty())
        {
            // Logic to use the item based on its type
            Debug.Log($"Using item: {storedItem.itemName}");

            // Example: If it's a consumable, decrease quantity
            if (storedItem.itemType == ItemEnums.ItemType.Consumable)
            {
                quantity--;
                if (quantity <= 0)
                {
                    ClearSlot();
                }
            }

            // Additional logic for other item types can be added here
        }
        else
        {
            Debug.Log("No item to use in this slot.");
        }
    }

    // can stack items in the slot
    public bool CanStack(Item item)
    {
        if (IsEmpty())
        {
            return false;
        }

        // Check if the items are of the same type and rareity and can be stacked
        return storedItem.itemName == item.itemName && storedItem.itemType == item.itemType && storedItem.itemRarity == item.itemRarity;
    }
    // Additional methods related to inventory slot functionality can be added here
}