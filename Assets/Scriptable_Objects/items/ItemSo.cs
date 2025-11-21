using UnityEngine;

[CreateAssetMenu]
public class ItemSo : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChange;

    public AttrubuteChangeType attrubuteChangeType = new AttrubuteChangeType();
    public int ammountToChangeAttribute;

    public void UseItem()
    {
        if (statToChange == StatToChange.health)
        {
            PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
            
            if (playerHealth != null)
            {
                playerHealth.Heal(amountToChange);
            }
            else
            {
                Debug.LogError("PlayerHealth component not found in the scene!");
            }
        }
    }

    public enum StatToChange
    {
        none,
        health,
        stamina,
        mana
    }

    public enum AttrubuteChangeType
    {
        none,
        strength,
        agility,
        intelligence,
        defence
    }
}
