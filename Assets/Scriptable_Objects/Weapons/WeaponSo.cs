using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponSO : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName = "Weapon";
    public Sprite weaponIcon;
    
    [Header("Combat Stats")]
    public int damage = 10;
    public float attackSpeed = 1f; // Attacks per second
    public float attackRange = 2f;
    
    [Header("Weapon Type")]
    public WeaponType weaponType = WeaponType.Melee;
    
    public enum WeaponType
    {
        Melee,
        Ranged
    }
}
