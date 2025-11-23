using UnityEngine;
// A simple Chest class that implements the I_Interactable interface. 
// When interacted with, it logs a message indicating that the chest has been opened.
public class Chest : MonoBehaviour, I_Interactable
{
    [SerializeField] private string _prompt; // the interaction prompt for the chest
    public string InteractionPrompt => _prompt; // implement the InteractionPrompt property

    // chest settings
    public ChestType chestType;
    private bool isOpened = false;
    private SpriteRenderer sr;



    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        
        if (chestType != null && sr != null)
        {
            sr.sprite = chestType.closedSprite;
        }
        else if (chestType == null)
        {
            Debug.LogWarning($"ChestType not assigned on {gameObject.name}. Please assign a ChestType ScriptableObject in the Inspector.");
        }
    }

    public void Open()
    {
        if (isOpened) return;
        isOpened = true;

        sr.sprite = chestType.openedSprite;


        // Additional logic for giving player gold can be added here
    }

    public bool Interact(Interactor interactor) // implement the Interact method
    {
        // Add chest interaction logic here
        Open();
        Debug.Log("Chest opened!");
        return true;
    }

}
