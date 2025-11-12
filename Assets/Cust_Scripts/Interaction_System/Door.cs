using UnityEngine;

public class Door : MonoBehaviour, I_Interactable
{
    [SerializeField] private string _prompt; // the interaction prompt for the door
    public string InteractionPrompt => _prompt; // implement the InteractionPrompt property
    public bool Interact(Interactor interactor) // implement the Interact method
    {
        // Add door interaction logic here
        Debug.Log("Door opened!");
        return true;
    }
}
