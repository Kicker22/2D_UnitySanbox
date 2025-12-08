using UnityEngine;

[CreateAssetMenu(fileName = "New Synthesis Item", menuName = "Items/Synthesis Item")]
public class SynthesisItemSO : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName = "Stone";
    public Sprite itemIcon;
    
    [TextArea(2, 4)]
    public string description = "A stone used for weapon synthesis";
    
    [Header("Synthesis Effects")]
    [Tooltip("How much this item increases each stat")]
    public int attackBoost = 0;
    public int speedBoost = 0;
    public int magicBoost = 0;
    public int enduranceBoost = 0;
    
    [Header("Elemental Effects")]
    public int fireAffinityBoost = 0;
    public int iceAffinityBoost = 0;
    public int lightningAffinityBoost = 0;
    public int windAffinityBoost = 0;
    public int holyAffinityBoost = 0;
    
    [Header("Monster Affinity Effects")]
    public int undeadAffinityBoost = 0;
    public int beastAffinityBoost = 0;
    public int aquaticAffinityBoost = 0;
    public int earthAffinityBoost = 0;
    public int plantAffinityBoost = 0;
    public int flyingAffinityBoost = 0;
    public int armorAffinityBoost = 0;
    public int mimicAffinityBoost = 0;
    public int mageAffinityBoost = 0;
    public int demonAffinityBoost = 0;
    public int reptileAffinityBoost = 0;
    
    [Header("Item Properties")]
    public int stackSize = 99;
    public int sellValue = 10;
    
    // Apply this synthesis item to a weapon
    public void ApplyToWeapon(WeaponSO weapon)
    {
        // Apply stat boosts
        if (attackBoost > 0) weapon.SynthesizeStat("attack", attackBoost);
        if (speedBoost > 0) weapon.SynthesizeStat("speed", speedBoost);
        if (magicBoost > 0) weapon.SynthesizeStat("magic", magicBoost);
        if (enduranceBoost > 0) weapon.SynthesizeStat("endurance", enduranceBoost);
        
        // Apply elemental boosts
        if (fireAffinityBoost > 0) weapon.SynthesizeElement("fire", fireAffinityBoost);
        if (iceAffinityBoost > 0) weapon.SynthesizeElement("ice", iceAffinityBoost);
        if (lightningAffinityBoost > 0) weapon.SynthesizeElement("lightning", lightningAffinityBoost);
        if (windAffinityBoost > 0) weapon.SynthesizeElement("wind", windAffinityBoost);
        if (holyAffinityBoost > 0) weapon.SynthesizeElement("holy", holyAffinityBoost);
        
        // Apply monster affinity boosts
        weapon.undeadAffinity += undeadAffinityBoost;
        weapon.beastAffinity += beastAffinityBoost;
        weapon.aquaticAffinity += aquaticAffinityBoost;
        weapon.earthAffinity += earthAffinityBoost;
        weapon.plantAffinity += plantAffinityBoost;
        weapon.flyingAffinity += flyingAffinityBoost;
        weapon.armorAffinity += armorAffinityBoost;
        weapon.mimicAffinity += mimicAffinityBoost;
        weapon.mageAffinity += mageAffinityBoost;
        weapon.demonAffinity += demonAffinityBoost;
        weapon.reptileAffinity += reptileAffinityBoost;
    }
}
