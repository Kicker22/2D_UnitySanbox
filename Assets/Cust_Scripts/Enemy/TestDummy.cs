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
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float wallAvoidanceDistance = 1.5f;

    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float damageFlashDuration = 0.2f;

    [Header("Loot Settings")]
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private int minGoldDrop = 10;
    [SerializeField] private int maxGoldDrop = 20;
    [SerializeField] private float dropRadius = 1.0f;

    // reference to animator
    [Header("Animator")]
    [SerializeField] private Animator _animator;

    // movment 
    private Vector2 movement;

    // testDummy specific health
    // [SerializeField] private float maxEnemyHealth = 100f;


    // Private variables
    private int currentHealth;
    private float lastAttackTime;
    private Transform player;
    private Rigidbody2D rb;
    private bool isPlayerInDetectionRange;
    private bool isPlayerInAttackRange;
    private bool isDead;
    
    // Pathfinding and stuck prevention
    private Vector2 lastPosition;
    private float stuckTimer = 0f;
    private float stuckCheckInterval = 0.5f;
    private float minimumMovementThreshold = 0.1f;
    private Vector2 unstuckDirection;
    private float unstuckDuration = 0f;
    private float maxUnstuckDuration = 1f;

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

        Vector2 direction;
        
        // If currently in unstuck mode, continue in unstuck direction
        if (unstuckDuration > 0f)
        {
            direction = unstuckDirection;
            unstuckDuration -= Time.deltaTime;
        }
        else
        {
            direction = (player.position - transform.position).normalized;
            
            // Check for obstacles and adjust direction
            direction = AvoidWalls(direction);
            
            // Detect if stuck and apply unstuck behavior
            if (IsStuck())
            {
                unstuckDirection = GetUnstuckDirection(direction);
                unstuckDuration = maxUnstuckDuration;
                direction = unstuckDirection;
            }
        }
        
        Vector2 velocity = direction * moveSpeed;
        rb.linearVelocity = velocity;
        movement = velocity;
        FlipSprite();

        // Update last position for stuck detection
        lastPosition = transform.position;
    }

    private Vector2 AvoidWalls(Vector2 desiredDirection)
    {
        // Multiple raycasts in different directions to detect corners better
        float[] angles = { 0f, 30f, -30f, 60f, -60f, 90f, -90f };
        Vector2 bestDirection = desiredDirection;
        float bestScore = -1f;
        
        foreach (float angle in angles)
        {
            Vector2 testDirection = Quaternion.Euler(0, 0, angle) * desiredDirection;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, testDirection, wallAvoidanceDistance, obstacleLayerMask);
            
            // Calculate score based on clearance and alignment with desired direction
            float clearance = hit.collider == null ? wallAvoidanceDistance : hit.distance;
            float alignment = Vector2.Dot(testDirection.normalized, desiredDirection);
            float score = clearance * (0.5f + alignment * 0.5f); // Weighted score
            
            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = testDirection.normalized;
            }
        }
        
        return bestDirection;
    }

    private bool IsStuck()
    {
        stuckTimer += Time.deltaTime;
        
        if (stuckTimer >= stuckCheckInterval)
        {
            stuckTimer = 0f;
            float distanceMoved = Vector2.Distance(lastPosition, transform.position);
            
            // If barely moved while trying to chase, we're stuck
            if (distanceMoved < minimumMovementThreshold && currentState == EnemyState.Chasing)
            {
                return true;
            }
        }
        
        return false;
    }

    private Vector2 GetUnstuckDirection(Vector2 currentDirection)
    {
        // Cast rays in all cardinal and diagonal directions to find the most open path
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2(1, 1).normalized, new Vector2(-1, 1).normalized,
            new Vector2(1, -1).normalized, new Vector2(-1, -1).normalized
        };
        
        Vector2 bestDirection = -currentDirection; // Default: go backwards
        float maxClearance = 0f;
        
        foreach (Vector2 dir in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, wallAvoidanceDistance * 2f, obstacleLayerMask);
            float clearance = hit.collider == null ? wallAvoidanceDistance * 2f : hit.distance;
            
            // Prefer directions that lead toward the player
            Vector2 toPlayer = (player.position - transform.position).normalized;
            float playerAlignment = Vector2.Dot(dir, toPlayer);
            float score = clearance * (1f + playerAlignment * 0.5f);
            
            if (score > maxClearance)
            {
                maxClearance = score;
                bestDirection = dir;
            }
        }
        
        Debug.Log($"{gameObject.name}: Stuck! Moving in direction {bestDirection} with clearance {maxClearance}");
        return bestDirection;
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

        // Drop gold
        DropGold();

        // Play death animation and destroy
        StartCoroutine(DeathSequence());
    }

    private void DropGold()
    {
        if (goldPrefab != null)
        {
            // Calculate random position within drop radius
            Vector2 randomOffset = Random.insideUnitCircle * dropRadius;
            Vector3 dropPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            
            // Spawn gold at random position
            GameObject droppedGold = Instantiate(goldPrefab, dropPosition, Quaternion.identity);
            
            // Set random gold amount
            int goldAmount = Random.Range(minGoldDrop, maxGoldDrop + 1);
            GoldPickUp goldPickup = droppedGold.GetComponent<GoldPickUp>();
            if (goldPickup != null)
            {
                goldPickup.SetGoldAmount(goldAmount);
                Debug.Log($"{gameObject.name} dropped {goldAmount} gold at position {dropPosition}!");
            }
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no gold prefab assigned!");
        }
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
