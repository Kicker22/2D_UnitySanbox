using UnityEngine;
using System.Collections;

public class TestDummy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private float attackCooldown = 1.5f;


    [Header("Behavior Settings")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private LayerMask playerLayerMask = 1;

    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float damageFlashDuration = 0.2f;

    // reference to animator
    [SerializeField] private Animator _animator;

    // movment 
    private Vector2 movement;

    // testDummy specific health
    [SerializeField] private float maxEnemyHealth = 100f;


    // Private variables
    private int currentHealth;
    private float lastAttackTime;
    private Transform player;
    private Rigidbody2D rb;
    private bool isPlayerInDetectionRange;
    private bool isPlayerInAttackRange;
    private bool isDead;

    // Enemy states
    public enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Dead
    }
    private EnemyState currentState = EnemyState.Idle;

    void Awake()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Initialize enemy stats
        currentHealth = maxHealth;

        // Ensure movement is enabled (override inspector if needed)
        canMove = true;

        // Find player GameObject (assumes player has "Player" tag)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log($"Player found: {player.name}");
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure player GameObject has 'Player' tag.");
        }

        // Check for required components
        if (rb == null)
        {
            Debug.LogError($"{gameObject.name}: Missing Rigidbody2D component!");
        }

        // Set initial color
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;

        Debug.Log($"{gameObject.name}: TestDummy initialized. CanMove: {canMove}, MoveSpeed: {moveSpeed}");
    }

    void Update()
    {
        if (isDead || player == null) return;

        CheckPlayerDistance();

        UpdateState();
        HandleBehavior();
        UpdateAnimations();
    }

    private void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        isPlayerInDetectionRange = distanceToPlayer <= detectionRadius;
        isPlayerInAttackRange = distanceToPlayer <= attackRange;
    }

    private void UpdateState()
    {
        EnemyState previousState = currentState;

        switch (currentState)
        {
            case EnemyState.Idle:
                if (isPlayerInDetectionRange)
                    currentState = EnemyState.Chasing;
                break;

            case EnemyState.Chasing:
                if (!isPlayerInDetectionRange)
                    currentState = EnemyState.Idle;
                else if (isPlayerInAttackRange && canAttack)
                    currentState = EnemyState.Attacking;
                break;

            case EnemyState.Attacking:
                if (!isPlayerInAttackRange)
                    currentState = EnemyState.Chasing;
                else if (!isPlayerInDetectionRange)
                    currentState = EnemyState.Idle;
                break;
        }

        // Debug state changes
        if (previousState != currentState)
        {
            Debug.Log($"{gameObject.name}: State changed from {previousState} to {currentState}");
        }
    }

    private void HandleBehavior()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                // Stop movement
                if (rb != null)
                    rb.linearVelocity = Vector2.zero;
                break;

            case EnemyState.Chasing:
                MoveTowardsPlayer();
                break;

            case EnemyState.Attacking:
                // Stop movement when attacking
                if (rb != null)
                    rb.linearVelocity = Vector2.zero;

                if (CanAttack())
                    Attack();
                break;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (!canMove)
        {
            Debug.Log($"{gameObject.name}: Can't move - canMove is false");
            return;
        }

        if (rb == null)
        {
            Debug.LogError($"{gameObject.name}: Can't move - no Rigidbody2D!");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning($"{gameObject.name}: Can't move - no player target!");
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 velocity = direction * moveSpeed;
        rb.linearVelocity = velocity;
        movement = velocity;
        FlipSprite();

        Debug.Log($"{gameObject.name}: Moving towards player. Direction: {direction}, Velocity: {velocity}");

        // Face the player (flip sprite if moving left)
        if (spriteRenderer != null)
        {
            if (direction.x < 0)
                spriteRenderer.flipX = true;
            else if (direction.x > 0)
                spriteRenderer.flipX = false;
        }
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

    private bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    private void Attack()
    {
        if (!canAttack || player == null) return;

        // Update attack time
        lastAttackTime = Time.time;

        // Try to get player health component and deal damage
        // First try the actual PlayerHealth component
        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log($"{gameObject.name} dealt {damage} damage to player!");
        }
        else
        {
            // Fallback: try the IHealth interface
            var iHealth = player.GetComponent<IHealth>();
            if (iHealth != null)
            {
                iHealth.TakeDamage(damage);
                Debug.Log($"{gameObject.name} dealt {damage} damage to player via IHealth!");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} attacked player but no health component found! Expected PlayerHealth or IHealth component.");
            }
        }

        // Visual feedback for attack
        StartCoroutine(AttackFlash());
    }

    private IEnumerator AttackFlash()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage! Health: {currentHealth}/{maxHealth}");

        // Visual feedback
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(damageFlashDuration);
            spriteRenderer.color = normalColor;
        }
    }

    private void Die()
    {
        isDead = true;
        currentState = EnemyState.Dead;

        Debug.Log($"{gameObject.name} died!");

        // Stop all movement
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Optional: Play death animation, drop loot, etc.
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Fade out or play death animation
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            float fadeTime = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
                spriteRenderer.color = color;
                yield return null;
            }
        }

        // Destroy the enemy
        Destroy(gameObject);
    }

    private void UpdateAnimations()
    {
        if (_animator == null) return;

        // Set animation parameters based on current state
        // Only use parameters that exist in your Animator Controller
        switch (currentState)
        {
            case EnemyState.Idle:
                SetAnimatorBool("isRunning", false);
                // SetAnimatorBool("isIdle", true);
                break;

            case EnemyState.Chasing:
                SetAnimatorBool("isRunning", true);
                // SetAnimatorBool("isIdle", false);
                break;

            case EnemyState.Attacking:
                SetAnimatorBool("isRunning", false);
                // You can add attack animation trigger here if you have one
                // SetAnimatorTrigger("Attack");
                break;

            case EnemyState.Dead:
                SetAnimatorBool("isRunning", false);
                // SetAnimatorBool("isDead", true);
                break;
        }
    }

    // Helper method to safely set animator parameters
    private void SetAnimatorBool(string paramName, bool value)
    {
        if (_animator == null) return;
        
        // Check if parameter exists before setting it
        foreach (AnimatorControllerParameter param in _animator.parameters)
        {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Bool)
            {
                _animator.SetBool(paramName, value);
                return;
            }
        }
        
        // Parameter doesn't exist - log warning but don't crash
        Debug.LogWarning($"Animator parameter '{paramName}' not found on {gameObject.name}");
    }

    // Helper method to safely trigger animator parameters
    private void SetAnimatorTrigger(string paramName)
    {
        if (_animator == null) return;
        
        // Check if parameter exists before setting it
        foreach (AnimatorControllerParameter param in _animator.parameters)
        {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Trigger)
            {
                _animator.SetTrigger(paramName);
                return;
            }
        }
        
        // Parameter doesn't exist - log warning but don't crash
        Debug.LogWarning($"Animator trigger '{paramName}' not found on {gameObject.name}");
    }

    // Public methods for external access
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsDead() => isDead;
    public EnemyState GetCurrentState() => currentState;

    // Gizmos for visualizing ranges in Scene view
    private void OnDrawGizmosSelected()
    {
        // Detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

// Interface for health system (optional - for better code organization)
public interface IHealth
{
    void TakeDamage(int damage);
    int GetCurrentHealth();
    int GetMaxHealth();
}
