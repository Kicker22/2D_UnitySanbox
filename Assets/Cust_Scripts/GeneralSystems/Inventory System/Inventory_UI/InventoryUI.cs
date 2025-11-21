using UnityEngine;

// Purpose: Handles the category tabs and slot display

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    private void Awake()
    {
        // Singleton pattern to ensure only one instance of InventoryUI exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Additional methods for managing the inventory UI can be added here

    // Example: Method to switch category tabs
    public void SwitchCategoryTab(string category)
    {
        // Logic to switch the displayed category tab in the inventory UI
        Debug.Log($"Switched to category tab: {category}");
        // Implementation would involve updating UI elements accordingly
    }

    // Example: Method to refresh inventory slots display
    public void RefreshInventorySlots()
    {
        // Logic to refresh the display of inventory slots
        Debug.Log("Inventory slots refreshed.");
        // Implementation would involve updating UI elements accordingly
    }

    // drag and drop functionality
    public void OnItemDrag(InventorySlot slot)
    {
        // Logic to handle item drag action
        Debug.Log($"Dragging item: {slot.GetItemInfo()}");
        // Implementation would involve UI drag-and-drop mechanics
    }
    public void OnItemDrop(InventorySlot fromSlot, InventorySlot toSlot)
    {
        // Logic to handle item drop action
        Debug.Log($"Dropped item from {fromSlot.GetItemInfo()} to {toSlot.GetItemInfo()}");
        // Implementation would involve swapping items between slots or merging stacks
    }
    
}