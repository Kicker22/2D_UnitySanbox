// This will handle equipping and managing the player's equipment
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Starting Equipment")]
    [SerializeField] private WeaponSO startingWeapon; // Optional: auto-equip on start

    [Header("Visual References")]
    [Tooltip("Create a child GameObject with SpriteRenderer under the player and assign it here")]
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    
    // Currently only handling weapons; can be expanded for armor/accessories
    private WeaponSO equippedWeapon; // Currently equipped weapon

    private void Start()
    {
        // Auto-equip starting weapon if assigned
        if (startingWeapon != null)
        {
            EquipWeapon(startingWeapon);
        }
    }

    // Equip a new weapon
    public void EquipWeapon(WeaponSO newWeapon)
    {
        equippedWeapon = newWeapon;
        Debug.Log($"Equipped weapon: {equippedWeapon.weaponName}");
        UpdateWeaponSprite();
    }

    // Unequip the current weapon
    public void UnequipWeapon()
    {
        if (equippedWeapon != null)
        {
            Debug.Log($"Unequipped weapon: {equippedWeapon.weaponName}");
        }
        equippedWeapon = null;
        UpdateWeaponSprite();
    }

    // Get the currently equipped weapon
    public WeaponSO GetEquippedWeapon()
    {
        return equippedWeapon;
    }

    // Update the weapon sprite based on the equipped weapon
    private void UpdateWeaponSprite()
    {
        if (weaponSpriteRenderer == null)
            return;

        if (equippedWeapon != null && equippedWeapon.weaponIcon != null)
        {
            weaponSpriteRenderer.sprite = equippedWeapon.weaponIcon;
            weaponSpriteRenderer.enabled = true;
        }
        else
        {
            weaponSpriteRenderer.sprite = null;
            weaponSpriteRenderer.enabled = false;
        }
    }
}