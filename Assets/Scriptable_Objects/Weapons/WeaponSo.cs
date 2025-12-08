using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponSO : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName = "Weapon";
    public Sprite weaponIcon;
    
    [Header("Weapon Type")]
    public WeaponType weaponType = WeaponType.Melee;
    
    [Header("Combat Stats (Calculated from Buildup)")]
    public float attackRange = 2f; // Attack range in world units
    
    [Header("Weapon Buildup Stats (Dark Cloud Style)")]
    [Tooltip("Current/Max values for each stat")]
    public int attack = 10;
    public int attackMax = 100;
    
    public int speed = 10;
    public int speedMax = 100;
    
    public int magic = 10;
    public int magicMax = 100;
    
    public int endurance = 10;
    public int enduranceMax = 100;
    
    // Calculated properties for combat
    public int damage => attack; // Damage is based on attack stat
    public float attackSpeed => 1f + (speed * 0.01f); // Attacks per second, scales with speed
    
    [Header("Weapon Experience & Level")]
    public int weaponExp = 0;
    public int weaponExpToNextLevel = 100; // XP needed for next level
    public int weaponLevel = 1;
    public int weaponLevelMax = 10;
    
    [Header("Evolution System")]
    [Tooltip("Multiple evolution paths - player chooses when ready")]
    public WeaponEvolutionPath[] evolutionPaths;
    
    [System.Serializable]
    public class WeaponEvolutionPath
    {
        public string evolutionName; // e.g., "Fire Blade", "Ice Sword"
        public WeaponSO targetWeapon;
        
        [Header("Stat Requirements")]
        public int attackRequired = 50;
        public int speedRequired = 50;
        public int magicRequired = 50;
        public int enduranceRequired = 50;
        
        [Header("Elemental Requirements (optional)")]
        public int fireAffinityRequired = 0;
        public int iceAffinityRequired = 0;
        public int lightningAffinityRequired = 0;
        public int windAffinityRequired = 0;
        public int holyAffinityRequired = 0;
    }

    [Header("Elemental Affinity")]
    public int fireAffinity;    // Increases fire damage or adds fire effect
    public int iceAffinity;     // Increases ice damage or adds ice effect
    public int lightningAffinity; // Increases lightning damage or adds lightning effect
    public int windAffinity;
    public int holyAffinity;

    [Header("Monster Affinity")]
    public int undeadAffinity;  // Increases damage against undead
    public int beastAffinity;
    public int aquaticAffinity;
    public int earthAffinity;
    public int plantAffinity;
    public int flyingAffinity;
    public int armorAffinity;
    public int mimicAffinity;
    public int mageAffinity;
    public int demonAffinity;
    public int reptileAffinity;



    public enum WeaponType
    {
        Melee,
        Ranged
    }
    
    // Add XP to weapon
    public bool AddExperience(int amount)
    {
        weaponExp += amount;
        
        // Level up if enough XP
        if (weaponExp >= weaponExpToNextLevel && weaponLevel < weaponLevelMax)
        {
            weaponLevel++;
            weaponExp -= weaponExpToNextLevel;
            weaponExpToNextLevel = Mathf.RoundToInt(weaponExpToNextLevel * 1.5f); // Increase XP requirement
            return true; // Leveled up
        }
        return false;
    }
    
    // Synthesis: Increase specific stat by amount
    public void SynthesizeStat(string statName, int amount)
    {
        switch (statName.ToLower())
        {
            case "attack":
                attack = Mathf.Min(attack + amount, attackMax);
                break;
            case "speed":
                speed = Mathf.Min(speed + amount, speedMax);
                break;
            case "magic":
                magic = Mathf.Min(magic + amount, magicMax);
                break;
            case "endurance":
                endurance = Mathf.Min(endurance + amount, enduranceMax);
                break;
        }
    }
    
    // Synthesis: Increase elemental affinity
    public void SynthesizeElement(string elementName, int amount)
    {
        switch (elementName.ToLower())
        {
            case "fire":
                fireAffinity += amount;
                break;
            case "ice":
                iceAffinity += amount;
                break;
            case "lightning":
                lightningAffinity += amount;
                break;
            case "wind":
                windAffinity += amount;
                break;
            case "holy":
                holyAffinity += amount;
                break;
        }
    }
    
    // Get all available evolution paths
    public List<WeaponEvolutionPath> GetAvailableEvolutions()
    {
        List<WeaponEvolutionPath> available = new List<WeaponEvolutionPath>();
        
        if (evolutionPaths == null) return available;
        
        foreach (var path in evolutionPaths)
        {
            if (CanEvolveInto(path))
            {
                available.Add(path);
            }
        }
        
        return available;
    }
    
    // Check if specific evolution path is available
    public bool CanEvolveInto(WeaponEvolutionPath path)
    {
        if (path.targetWeapon == null) return false;
        
        bool statsOk = attack >= path.attackRequired &&
                       speed >= path.speedRequired &&
                       magic >= path.magicRequired &&
                       endurance >= path.enduranceRequired;
        
        bool elementsOk = fireAffinity >= path.fireAffinityRequired &&
                          iceAffinity >= path.iceAffinityRequired &&
                          lightningAffinity >= path.lightningAffinityRequired &&
                          windAffinity >= path.windAffinityRequired &&
                          holyAffinity >= path.holyAffinityRequired;
        
        return statsOk && elementsOk;
    }
    
    // Helper method to get total stat percentage (for visual progress bars)
    public float GetStatCompletion()
    {
        float attackPercent = (float)attack / attackMax;
        float speedPercent = (float)speed / speedMax;
        float magicPercent = (float)magic / magicMax;
        float endurancePercent = (float)endurance / enduranceMax;
        
        return (attackPercent + speedPercent + magicPercent + endurancePercent) / 4f;
    }
}
