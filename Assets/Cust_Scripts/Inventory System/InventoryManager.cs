using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject InventoryMenu;

    [Header("Input Actions")]
    public InputActionReference toggleInventoryAction; // Drag your Tab action here in Inspector

    [Header("Pause Settings")]
    public bool pauseGameWhenOpen = true; // Toggle this in Inspector if needed

    private bool menuActivated = false;
    private float previousTimeScale = 1f; // Remember the timescale before pausing

    public ItemSlot[] itemSlot;
    public ItemSo[] itemSos;

    private void OnEnable()
    {
        // Subscribe to the input action when this object becomes active
        toggleInventoryAction.action.performed += OnToggleInventory;
    }

    private void OnDisable()
    {
        // Unsubscribe when this object becomes inactive
        toggleInventoryAction.action.performed -= OnToggleInventory;
    }

    void Start()
    {
        // Make sure inventory starts closed
        InventoryMenu.SetActive(false);
        menuActivated = false;
    }

    // This method gets called when Tab is pressed
    private void OnToggleInventory(InputAction.CallbackContext context)
    {
        ToggleInventory();
    }

    // Toggle the inventory on/off
    public void ToggleInventory()
    {
        menuActivated = !menuActivated;
        InventoryMenu.SetActive(menuActivated);

        // Pause/Resume game based on inventory state
        if (pauseGameWhenOpen)
        {
            if (menuActivated)
            {
                previousTimeScale = Time.timeScale; // Remember current timescale
                Time.timeScale = 0f; // Pause the game
                Debug.Log("Inventory Opened - Game Paused");
            }
            else
            {
                Time.timeScale = previousTimeScale; // Restore previous timescale
                Debug.Log("Inventory Closed - Game Resumed");
            }
        }
        else
        {
            Debug.Log($"Inventory {(menuActivated ? "Opened" : "Closed")} - Game Not Paused");
        }
    }

    public void UseItem(string itemName)
    {
        // Loop through the itemSos array, not itemSlot array
        for (int i = 0; i < itemSos.Length; i++)
        {
            if (itemSos[i] != null && itemSos[i].itemName == itemName)
            {
                itemSos[i].UseItem();
                return;
            }
        }
    }
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            // Check if slot is empty OR has same item and isn't full
            bool isEmptySlot = itemSlot[i].quantity == 0;
            bool isSameItemNotFull = (itemSlot[i].itemName == itemName && itemSlot[i].isFull == false);
            
            if (isEmptySlot || isSameItemNotFull)
            {
                int lefOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                
                if (lefOverItems > 0)
                {
                    lefOverItems = AddItem(itemName, lefOverItems, itemSprite, itemDescription);
                }

                return lefOverItems;
            }
        }
        
        return quantity;
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
