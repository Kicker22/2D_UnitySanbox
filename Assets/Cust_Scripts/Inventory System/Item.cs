using UnityEngine;

public class Item : MonoBehaviour, I_Interactable
{
    [SerializeField] public string itemName;  // Made public so InventorySlot can access it
    [SerializeField] private int quantity;
    [SerializeField] public Sprite sprite;  // Made public for easier access

    [TextArea]
    [SerializeField] public string itemDescription;  // Also made public for consistency

    // New enum-based properties (keeping both for compatibility)
    public ItemEnums.ItemType itemType; // Type of the item
    public ItemEnums.Rarity itemRarity; // Rarity of the item
    public Sprite itemIcon; // Icon representing the item
    public ItemEnums.EquipmentType equipmentType; // Type of equipment (if applicable)
    public ItemEnums.WeaponType weaponType; // Type of weapon (if applicable)
    public ItemEnums.ConsumableType consumableType; // Type of consumable (if applicable)
    public ItemEnums.QuestItemType questItemType; // Type of quest item (if applicable)
    public ItemEnums.MiscellaneousType miscellaneousType; // Type of miscellaneous item (if applicable)
    public ItemEnums.ToolType toolType; // Type of tool (if applicable)

    private InventoryManager inventoryManager;
    
    void Start()
    {
        GameObject canvasObj = GameObject.Find("InventoryCanvas");
        
        if (canvasObj != null)
        {
            inventoryManager = canvasObj.GetComponent<InventoryManager>();
        }
        else
        {
            Debug.LogError($"Item {itemName}: InventoryCanvas GameObject not found!");
        }
    }

    // I_Interactable implementation
    public string InteractionPrompt => $"Pick up {itemName} (x{quantity})";

    public bool Interact(Interactor interactor)
    {
        if (inventoryManager != null)
        {
            // Use sprite if assigned, otherwise use itemIcon as fallback
            Sprite spriteToUse = sprite != null ? sprite : itemIcon;
            
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, spriteToUse, itemDescription);
            
            if (leftOverItems <= 0)
            {
                // Successfully picked up all items
                Destroy(gameObject);
                return true;
            }
            else
            {
                // Partially picked up, update quantity
                quantity = leftOverItems;
                return true; // Still successful interaction, just partial
            }
        }
        else
        {
            Debug.LogError($"Item {itemName}: InventoryManager is null! Cannot add to inventory.");
            return false;
        }
    }
}