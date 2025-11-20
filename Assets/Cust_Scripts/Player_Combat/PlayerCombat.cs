using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    private InputAction attackAction;

    private void Awake()
    {
        // Initialize the action in Awake (called before OnEnable)
        attackAction = InputSystem.actions.FindAction("Attack");
        
        if (attackAction == null)
        {
            Debug.LogWarning("Attack action not found in Input System. Make sure 'Attack' action exists in your Input Actions asset.");
        }
    }

    private void OnEnable()
    {
        if (attackAction != null)
        {
            attackAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (attackAction != null)
        {
            attackAction.Disable();
        }
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        PerformAttack();
        Debug.Log("Attack performed!");
    }

    private void PerformAttack()
    {
        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage each enemy hit
        foreach (Collider2D enemy in hitEnemies)
        {
            // Try to get TestDummy component
            TestDummy testDummy = enemy.GetComponent<TestDummy>();
            if (testDummy != null)
            {
                testDummy.TakeDamage(attackDamage);
                Debug.Log($"Hit {enemy.name} for {attackDamage} damage!");
            }

            // You can add more enemy types here
            // EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            // if (enemyHealth != null) { enemyHealth.TakeDamage(attackDamage); }
        }
    }

    // Visualize attack range in Scene view
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }    // Update is called once per frame
    void Update()
    {
        if (attackAction != null && attackAction.triggered)
        {
            OnAttackPerformed(new InputAction.CallbackContext());
        }
    }
}
