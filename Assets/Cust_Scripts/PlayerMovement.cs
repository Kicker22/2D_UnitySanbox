using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;

    private Vector2 movement;
    private Rigidbody2D rb;

    // Input action reference
    InputAction moveAction;


    // reference to animator
    [SerializeField] private Animator _animator;
    
    // reference to sprite renderer for flipping
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        // init 2d movement
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();

        // Ensure we have a rigidBody2D set to player
        if (rb == null)
        {
            Debug.LogError("RigidBody2D component missing from this gameobject. Please add one.");
        }
        
        // Get sprite renderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void FixedUpdate()
    {
        // Get input from the input system
        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        // store the movement for smooth movment
        movement = inputVector.normalized;

        if (movement.magnitude >= 0.1f)
        {
            Vector2 targetVelocity = movement * moveSpeed;
            rb.linearVelocity = targetVelocity;

            // Set running animation
            _animator.SetBool("isRunning", true);
            // _animator.SetBool("isIdle", false);
            
            // Flip sprite based on horizontal movement direction
            FlipSprite();
        }
        else
        {
            // Stop the player when no input is detected
            rb.linearVelocity = Vector2.zero;

            // Set idle animation
            _animator.SetBool("isRunning", false);
            // _animator.SetBool("isIdle", true);
        }
    }

    public Vector2 GetMovement()
    {
        return movement;
    }
    
    private void FlipSprite()
    {
        // Only flip on horizontal movement (left/right)
        if (movement.x > 0.1f)
        {
            // Moving right - face right (normal sprite)
            spriteRenderer.flipX = false;
        }
        else if (movement.x < -0.1f)
        {
            // Moving left - flip sprite horizontally
            spriteRenderer.flipX = true;
        }
        // Don't flip if only moving up/down (movement.x near 0)
    }
}
