// This will handle equipping and managing the player's equipment
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{

    // Get a reference to the  weapon sprite renderer
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    
    // Currently only handling weapons; can be expanded for armor/accessories
    private WeaponSO equippedWeapon; // Currently equipped weapon

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
        Debug.Log($"Unequipped weapon: {equippedWeapon.weaponName}");
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
        if (equippedWeapon != null && weaponSpriteRenderer != null)
        {
            weaponSpriteRenderer.sprite = equippedWeapon.weaponIcon;
            weaponSpriteRenderer.enabled = true;
        }
        else if (weaponSpriteRenderer != null)
        {
            weaponSpriteRenderer.sprite = null;
            weaponSpriteRenderer.enabled = false;
        }
    }
}