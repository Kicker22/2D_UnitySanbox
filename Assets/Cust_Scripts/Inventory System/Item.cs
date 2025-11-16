using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public string itemName;  // Made public so InventorySlot can access it
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;

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
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
            if (leftOverItems <= 0)
                Destroy(gameObject);
            else
                quantity = leftOverItems;
        }
    }
}