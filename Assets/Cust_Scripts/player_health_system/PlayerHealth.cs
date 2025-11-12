using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public event System.Action<int, int> OnHealthChanged;
    
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount <= 0) return;

        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Heal(int healAmount)
    {
        if (healAmount <= 0) return;

        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(10);
        }
    }
}
