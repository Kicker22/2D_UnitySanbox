using UnityEngine;
// A simple Chest class that implements the I_Interactable interface. 
// When interacted with, it logs a message indicating that the chest has been opened.
public class Chest : MonoBehaviour, I_Interactable
{
    [SerializeField] private string _prompt; // the interaction prompt for the chest
    public string InteractionPrompt => _prompt; // implement the InteractionPrompt property
    public bool Interact(Interactor interactor) // implement the Interact method
    {
        // Add chest interaction logic here
        Debug.Log("Chest opened!");
        return true;
    }

}
