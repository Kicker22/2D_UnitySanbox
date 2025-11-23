using UnityEngine;

/// <summary>
/// Place this on weapon pickup items in the world.
/// When the player collides with it, they'll automatically equip the weapon.
/// </summary>
public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] private WeaponSO weaponData;
    
    [Header("Optional Settings")]
    [SerializeField] private bool destroyOnPickup = true;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player touched this
        if (other.CompareTag("Player"))
        {
            PlayerEquipment playerEquipment = other.GetComponent<PlayerEquipment>();
            
            if (playerEquipment != null && weaponData != null)
            {
                playerEquipment.EquipWeapon(weaponData);
                
                if (destroyOnPickup)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
