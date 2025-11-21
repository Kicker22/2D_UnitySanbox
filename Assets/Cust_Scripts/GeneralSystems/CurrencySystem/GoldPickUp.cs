
using UnityEngine;

public class GoldPickUp : MonoBehaviour
{
    [Header("Gold Pickup Settings")]
    [SerializeField] private int goldAmount = 10;
    // [SerializeField] private AudioClip pickUpSound;
    // private AudioSource audioSource;

    private void Awake()
    {
        // audioSource = GetComponent<AudioSource>();
    }

    // Method to set gold amount (called by enemies when dropping)
    public void SetGoldAmount(int amount)
    {
        goldAmount = amount;
    }

    // Detect player collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Gold trigger hit by: {collision.gameObject.name} with tag: {collision.tag}");
        
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player detected! Attempting to add gold...");
            
            if (PlayerGoldManager.Instance != null)
            {
                PlayerGoldManager.Instance.AddGold(goldAmount);
                Debug.Log("Gold added successfully!");
            }
            else
            {
                Debug.LogError("PlayerGoldManager.Instance is null!");
            }
            
            Destroy(gameObject);
        }
    }
}
