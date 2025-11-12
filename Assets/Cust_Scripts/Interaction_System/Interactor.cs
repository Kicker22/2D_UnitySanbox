using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;

    private readonly Collider2D[] _colliders = new Collider2D[3];
    [SerializeField] private int _numFound;

    [SerializeField] private Interaction_UI _interactionUI;

    // check for number of colliders found

    private I_Interactable _interactable;

    private void Awake()
    {
        // Validate required components
        if (_interactionPoint == null)
        {
            Debug.LogError("Interaction Point is not assigned in the Interactor component!", this);
        }
        
        if (_interactionUI == null)
        {
            Debug.LogError("Interaction UI is not assigned in the Interactor component!", this);
        }
    }

    private void Update()
    {
        // Null check for required components
        if (_interactionPoint == null || _interactionUI == null)
        {
            return;
        }

        // Check for colliders in interaction radius 
        var foundColliders = Physics2D.OverlapCircleAll(_interactionPoint.position, _interactionRadius, _interactableMask); 

        _numFound = foundColliders.Length; // check for number of colliders found

        // Store up to 3 colliders in our array for potential use
        System.Array.Clear(_colliders, 0, _colliders.Length);
        for (int i = 0; i < _colliders.Length && i < foundColliders.Length; i++)
        {
            _colliders[i] = foundColliders[i];
        }

        // If we found at least one interactable, try to interact with the first one
        if (_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<I_Interactable>();

            if (_interactable != null)
            {
                if (!_interactionUI.isDisplayed) 
                {
                    _interactionUI.SetUp(_interactable.InteractionPrompt);
                }

                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    _interactable.Interact(this);
                }
            }
        }
        else
        {
            // No interactables found, close UI and clear reference
            if (_interactable != null) 
            {
                _interactable = null;
            }
            if (_interactionUI.isDisplayed) 
            {
                _interactionUI.Close();
            }
        }
    }

    // Visualize interaction radius in editor
    void OnDrawGizmos()
    {
        if (_interactionPoint == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionRadius);
    }
}
