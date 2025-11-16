using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{

    // Item data
    public string itemName; // item name
    public int quantity;    // item quantity
    public Sprite itemSprite; // item sprite
    public bool isFull; // whether the slot is full
    public string itemDescription; // item description
    public Sprite emptySprite; // empty slot sprite

    [SerializeField] private int maxNumberOfItems;


    // item slot
    [SerializeField] TMP_Text quantityText;
    [SerializeField] Image itemImage;

    // Desc slot info
    public Image itemDescImage;
    public TMP_Text itemDescName;
    public TMP_Text itemDescText;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        // check to see if slot already full
        if (isFull)
            return quantity;

        this.itemName = itemName; // set item name
        this.itemSprite = itemSprite; // set item sprite
        
        // Try to find itemImage if it's not assigned
        if (itemImage == null)
        {
            itemImage = GetComponentInChildren<Image>();
        }
        
        if (itemImage != null)
        {
            itemImage.sprite = itemSprite;
        }
        else
        {
            Debug.LogError("ItemSlot: itemImage is null! No Image component found in children.");
        }

        this.itemDescription = itemDescription; // set item description

        // Update quantity
        this.quantity += quantity;

        if (this.quantity > maxNumberOfItems)
        {
            if (quantityText == null)
            {
                quantityText = GetComponentInChildren<TMP_Text>();
            }
            
            if (quantityText != null)
            {
                quantityText.text = maxNumberOfItems.ToString();
                quantityText.enabled = true;
            }
            
            isFull = true; // mark slot as full

            // return the leftovers
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        // Update quantity text
        if (quantityText == null)
        {
            quantityText = GetComponentInChildren<TMP_Text>();
        }
        
        if (quantityText != null)
        {
            quantityText.text = this.quantity.ToString();
            quantityText.enabled = true;
        }
        else
        {
            Debug.LogError("ItemSlot: quantityText is null! No TMP_Text component found in children.");
        }

        return 0; // no leftovers
    }


    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnPointerClick(eventData);
    }

    void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick() // controller for left click behavior in inventory
    {
        if (thisItemSelected)
            inventoryManager.UseItem(itemName);

            
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        itemDescName.text = itemName;
        itemDescText.text = itemDescription;
        itemDescImage.sprite = itemSprite;

        if (itemDescImage.sprite == null)
        {
            itemDescImage.sprite = emptySprite;
        }
    }

    public void OnRightClick()
    {
        Debug.Log("Right Clicked on " + itemName);
        // Implement right click behavior (e.g., drop item)
    }


}