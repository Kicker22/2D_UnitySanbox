using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public event System.Action<int, int> OnHealthChanged;

    private PlayerHealth playerHealth;

    void Start() 
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerHealth.OnHealthChanged += HandleHealthChange;
    }

    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= HandleHealthChange;
    }

    private void HandleHealthChange(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }
}