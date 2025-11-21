using UnityEngine;

public class Door : MonoBehaviour, I_Interactable
{
    [Header("Door Settings")]
    [SerializeField] private bool isLocked = false;
    [SerializeField] private bool requiresKey = false;
    [SerializeField] private string requiredKeyID = "";

    [Header("Interaction Settings")]
    [SerializeField] private string lockedPrompt = "Press E to unlock (Requires Key)";
    [SerializeField] private string unlockedPrompt = "Press E to open";
    [SerializeField] private string openPrompt = "Press E to close";

    [Header("Visual Settings")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Collision Settings")]
    [SerializeField] private Collider2D doorCollider;

    private bool isOpen = false;

    public string InteractionPrompt
    {
        get
        {
            if (isLocked)
                return lockedPrompt;
            else if (isOpen)
                return openPrompt;
            else
                return unlockedPrompt;
        }
    }

    private void Awake()
    {
        // Auto-assign components if not set
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (doorCollider == null)
            doorCollider = GetComponent<Collider2D>();
    }

    public bool Interact(Interactor interactor)
    {
        // If door is locked, try to unlock it
        if (isLocked)
        {
            if (requiresKey)
            {
                // Check if player has the required key
                if (HasKey(interactor))
                {
                    UnlockDoor();
                    return true;
                }
                else
                {
                    Debug.Log("Door is locked. You need a key!");
                    return false;
                }
            }
            else
            {
                // Door doesn't require a key - unlock it directly
                UnlockDoor();
                OpenDoor();
                return true;
            }
        }

        // If door is unlocked, toggle open/close
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }

        return true;
    }

    public bool CanInteract(Interactor interactor)
    {
        // Can always try to interact with a door
        return true;
    }

    private bool HasKey(Interactor interactor)
    {
        // Try to get an inventory component from the interactor
        // This is a placeholder - replace with your actual inventory system
        // Example: PlayerInventory inventory = interactor.GetComponent<PlayerInventory>();
        // return inventory != null && inventory.HasItem(requiredKeyID);
        
        Debug.Log($"Checking for key: {requiredKeyID}");
        return false; // Replace with actual key check
    }

    private void OpenDoor()
    {
        isOpen = true;

        // Disable collision so player can pass through
        if (doorCollider != null)
            doorCollider.enabled = false;

        // Update visual representation
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetBool("isOpen", true);
        }
        else if (spriteRenderer != null && openSprite != null)
        {
            spriteRenderer.sprite = openSprite;
        }

        Debug.Log("Door opened!");
    }

    private void CloseDoor()
    {
        isOpen = false;

        // Enable collision to block player
        if (doorCollider != null)
            doorCollider.enabled = true;

        // Update visual representation
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetBool("isOpen", false);
        }
        else if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }

        Debug.Log("Door closed!");
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("Door unlocked!");
        
        // Optionally open the door immediately after unlocking
        // OpenDoor();
    }

    public void LockDoor()
    {
        isLocked = true;
        // Close door when locking
        if (isOpen)
            CloseDoor();
        Debug.Log("Door locked!");
    }

    // Public methods for external access
    public bool IsOpen() => isOpen;
    public bool IsLocked() => isLocked;
    public void SetRequiresKey(bool requires, string keyID = "")
    {
        requiresKey = requires;
        requiredKeyID = keyID;
    }
}
