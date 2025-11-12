using UnityEngine;
using UnityEngine.UIElements;

public class GameUIHandler : MonoBehaviour
{
    public PlayerControl PlayerControl;
    public UIDocument UIDoc;

    private VisualElement healthBarFill;

    void Start()
    {
        // Find the health bar fill element
        healthBarFill = UIDoc.rootVisualElement.Q<VisualElement>("HealthBarFill");
        
        if (healthBarFill == null)
        {
            Debug.LogError("HealthBarFill not found in UI!");
            return;
        }

        // Subscribe to health changes
        PlayerControl.OnHealthChanged += UpdateHealthBar;
    }

    void OnDestroy()
    {
        if (PlayerControl != null)
            PlayerControl.OnHealthChanged -= UpdateHealthBar;
    }

    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthBarFill == null || maxHealth == 0) return;

        float healthPercent = ((float)currentHealth / maxHealth) * 100f;
        healthBarFill.style.width = Length.Percent(healthPercent);
        
        Debug.Log($"Health Bar: {currentHealth}/{maxHealth} = {healthPercent:F1}%");
    }
}