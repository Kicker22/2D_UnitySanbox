
// this interface is for all interactable objects in the game
// it defines a prompt string and an interact method that takes an interactor as a parameter 
// and returns a boolean indicating whether the interaction was successful

//------- any object that implements this interface can be interacted with by an interactor -----------// 
public interface I_Interactable
{
    public string InteractionPrompt { get; }

    public bool Interact(Interactor interactor);
}