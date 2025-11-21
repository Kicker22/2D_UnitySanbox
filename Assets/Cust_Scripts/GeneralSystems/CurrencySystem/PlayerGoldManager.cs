using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{
    [Header("Gold Settings")]
    [SerializeField] private int startingGold = 100;

    private int currentGold;
    private static PlayerGoldManager instance;

    // Property to access current gold amount we use Instance pattern
    // because we want to ensure there's only one PlayerGoldManager
    public static PlayerGoldManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<PlayerGoldManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PlayerGoldManager");
                    instance = obj.AddComponent<PlayerGoldManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Ensure singleton instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Initialize current gold
        currentGold = startingGold;
    }

    public int currentGoldAmount
    {
        get { return currentGold; }
    }

    public void AddGold(int amount)
    {
        if (amount > 0)
        {
            currentGold += amount;
            Debug.Log($"Added {amount} gold. Current gold: {currentGold}");
        }
    }

    public bool SpendGold(int amount)
    {
        if (amount > 0 && currentGold >= amount)
        {
            currentGold -= amount;
            Debug.Log($"Spent {amount} gold. Current gold: {currentGold}");
            return true;
        }
        else
        {
            Debug.Log("Not enough gold to spend!");
            return false;
        }
    }

    
}
