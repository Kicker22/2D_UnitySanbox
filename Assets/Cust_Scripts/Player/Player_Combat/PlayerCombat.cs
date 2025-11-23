using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private LayerMask enemyLayers;

    [Header("References")]
    [SerializeField] private PlayerEquipment playerEquipment;
    

    private InputAction attackAction;
    private WeaponSO equippedWeapon; // Cached reference from PlayerEquipment

    private void Awake()
    {
        // Initialize the action in Awake (called before OnEnable)
        attackAction = InputSystem.actions.FindAction("Attack");
        
        if (attackAction == null)
        {
            Debug.LogWarning("Attack action not found in Input System. Make sure 'Attack' action exists in your Input Actions asset.");
        }

        // Get PlayerEquipment if not assigned
        if (playerEquipment == null)
        {
            playerEquipment = GetComponent<PlayerEquipment>();
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
        // Get the current weapon from PlayerEquipment
        equippedWeapon = playerEquipment?.GetEquippedWeapon();

        // Check if we have a weapon equipped
        if (equippedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped! Cannot attack.");
            return;
        }

        // Detect enemies in range using weapon's attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackOrigin.position, equippedWeapon.attackRange, enemyLayers);

        // Damage each enemy hit
        foreach (Collider2D enemy in hitEnemies)
        {
            // Try to get TestDummy component
            TestDummy testDummy = enemy.GetComponent<TestDummy>();
            if (testDummy != null)
            {
                testDummy.TakeDamage(equippedWeapon.damage);
                Debug.Log($"Hit {enemy.name} for {equippedWeapon.damage} damage with {equippedWeapon.weaponName}!");
            }

            // You can add more enemy types here
            // EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            // if (enemyHealth != null) { enemyHealth.TakeDamage(equippedWeapon.damage); }
        }
    }

    // Visualize attack range in Scene view
    private void OnDrawGizmosSelected()
    {
        if (attackOrigin == null)
            return;

        // Get equipped weapon from PlayerEquipment for visualization
        WeaponSO weapon = playerEquipment?.GetEquippedWeapon();
        float range = weapon != null ? weapon.attackRange : 2f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position, range);
    }    // Update is called once per frame
    void Update()
    {
        if (attackAction != null && attackAction.triggered)
        {
            OnAttackPerformed(new InputAction.CallbackContext());
        }
    }
}
