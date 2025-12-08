using UnityEngine;

/// <summary>
/// Test script for weapon buildup system - attach to player
/// Uses keyboard shortcuts to test synthesis and evolution
/// </summary>
public class WeaponBuildupTester : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEquipment playerEquipment;
    
    [Header("Test Items")]
    [SerializeField] private SynthesisItemSO testAttackStone;
    [SerializeField] private SynthesisItemSO testSpeedStone;
    [SerializeField] private SynthesisItemSO testMagicStone;
    [SerializeField] private SynthesisItemSO testFireGem;

    private void Awake()
    {
        if (playerEquipment == null)
        {
            playerEquipment = GetComponent<PlayerEquipment>();
        }
    }

    private void Update()
    {
        WeaponSO currentWeapon = playerEquipment?.GetEquippedWeapon();
        if (currentWeapon == null)
        {
            Debug.LogWarning("No weapon equipped!");
            return;
        }

        // Add XP with 'X' key
        if (Input.GetKeyDown(KeyCode.X))
        {
            bool leveled = currentWeapon.AddExperience(50);
            Debug.Log($"[XP] Added 50 XP. Total: {currentWeapon.weaponExp}/{currentWeapon.weaponExpToNextLevel}");
            if (leveled)
            {
                Debug.Log($"<color=yellow>LEVEL UP! Now Level {currentWeapon.weaponLevel}</color>");
            }
        }

        // Add Attack (with item or directly)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (testAttackStone != null)
            {
                testAttackStone.ApplyToWeapon(currentWeapon);
                Debug.Log($"[SYNTHESIS] Applied {testAttackStone.itemName}");
            }
            else
            {
                currentWeapon.SynthesizeStat("attack", 5);
                Debug.Log($"[STAT] Attack +5");
            }
            LogWeaponStats(currentWeapon);
        }

        // Add Speed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (testSpeedStone != null)
            {
                testSpeedStone.ApplyToWeapon(currentWeapon);
                Debug.Log($"[SYNTHESIS] Applied {testSpeedStone.itemName}");
            }
            else
            {
                currentWeapon.SynthesizeStat("speed", 5);
                Debug.Log($"[STAT] Speed +5");
            }
            LogWeaponStats(currentWeapon);
        }

        // Add Magic
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (testMagicStone != null)
            {
                testMagicStone.ApplyToWeapon(currentWeapon);
                Debug.Log($"[SYNTHESIS] Applied {testMagicStone.itemName}");
            }
            else
            {
                currentWeapon.SynthesizeStat("magic", 5);
                Debug.Log($"[STAT] Magic +5");
            }
            LogWeaponStats(currentWeapon);
        }

        // Add Endurance
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentWeapon.SynthesizeStat("endurance", 5);
            Debug.Log($"[STAT] Endurance +5");
            LogWeaponStats(currentWeapon);
        }

        // Add Fire Affinity
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (testFireGem != null)
            {
                testFireGem.ApplyToWeapon(currentWeapon);
                Debug.Log($"[SYNTHESIS] Applied {testFireGem.itemName}");
            }
            else
            {
                currentWeapon.SynthesizeElement("fire", 10);
                Debug.Log($"[ELEMENT] Fire +10");
            }
            LogWeaponStats(currentWeapon);
        }

        // Check Evolution Paths
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckEvolutionPaths(currentWeapon);
        }

        // Print Full Stats
        if (Input.GetKeyDown(KeyCode.S))
        {
            LogFullStats(currentWeapon);
        }
    }

    private void CheckEvolutionPaths(WeaponSO weapon)
    {
        Debug.Log($"<color=cyan>===== EVOLUTION CHECK: {weapon.weaponName} =====</color>");
        
        var availablePaths = weapon.GetAvailableEvolutions();
        
        if (availablePaths.Count == 0)
        {
            Debug.Log("<color=yellow>No evolution paths available yet.</color>");
            
            // Show what's needed
            if (weapon.evolutionPaths != null && weapon.evolutionPaths.Length > 0)
            {
                Debug.Log("\n<color=orange>Requirements:</color>");
                foreach (var path in weapon.evolutionPaths)
                {
                    Debug.Log($"  {path.evolutionName} → {path.targetWeapon?.weaponName ?? "null"}");
                    Debug.Log($"    Attack: {weapon.attack}/{path.attackRequired}");
                    Debug.Log($"    Speed: {weapon.speed}/{path.speedRequired}");
                    Debug.Log($"    Magic: {weapon.magic}/{path.magicRequired}");
                    Debug.Log($"    Endurance: {weapon.endurance}/{path.enduranceRequired}");
                    if (path.fireAffinityRequired > 0)
                        Debug.Log($"    Fire: {weapon.fireAffinity}/{path.fireAffinityRequired}");
                }
            }
        }
        else
        {
            Debug.Log($"<color=green>✓ {availablePaths.Count} Evolution Path(s) Available!</color>");
            foreach (var path in availablePaths)
            {
                Debug.Log($"  → {path.evolutionName}: {path.targetWeapon?.weaponName}");
            }
        }
    }

    private void LogWeaponStats(WeaponSO weapon)
    {
        Debug.Log($"<color=lime>ATK:{weapon.attack}/{weapon.attackMax} SPD:{weapon.speed}/{weapon.speedMax} MAG:{weapon.magic}/{weapon.magicMax} END:{weapon.endurance}/{weapon.enduranceMax}</color>");
    }

    private void LogFullStats(WeaponSO weapon)
    {
        Debug.Log($"<color=cyan>===== {weapon.weaponName} (Lvl {weapon.weaponLevel}) =====</color>");
        Debug.Log($"XP: {weapon.weaponExp}/{weapon.weaponExpToNextLevel}");
        Debug.Log($"Damage: {weapon.damage} | Attack Speed: {weapon.attackSpeed:F2} | Range: {weapon.attackRange}");
        Debug.Log($"Attack: {weapon.attack}/{weapon.attackMax}");
        Debug.Log($"Speed: {weapon.speed}/{weapon.speedMax}");
        Debug.Log($"Magic: {weapon.magic}/{weapon.magicMax}");
        Debug.Log($"Endurance: {weapon.endurance}/{weapon.enduranceMax}");
        Debug.Log($"Fire: {weapon.fireAffinity} | Ice: {weapon.iceAffinity} | Lightning: {weapon.lightningAffinity}");
        Debug.Log($"Stat Completion: {weapon.GetStatCompletion() * 100f:F1}%");
    }

    private void OnGUI()
    {
        // Show controls on screen
        GUI.color = Color.white;
        GUILayout.BeginArea(new Rect(10, 10, 300, 400));
        GUILayout.Label("<b>WEAPON BUILDUP TESTER</b>");
        GUILayout.Label("X = Add 50 XP");
        GUILayout.Label("1 = +Attack");
        GUILayout.Label("2 = +Speed");
        GUILayout.Label("3 = +Magic");
        GUILayout.Label("4 = +Endurance");
        GUILayout.Label("F = +Fire Affinity");
        GUILayout.Label("E = Check Evolution");
        GUILayout.Label("S = Show Full Stats");
        
        WeaponSO weapon = playerEquipment?.GetEquippedWeapon();
        if (weapon != null)
        {
            GUILayout.Space(10);
            GUILayout.Label($"<b>{weapon.weaponName}</b> Lvl {weapon.weaponLevel}");
            GUILayout.Label($"DMG: {weapon.damage} | AS: {weapon.attackSpeed:F2}");
            GUILayout.Label($"ATK: {weapon.attack}/{weapon.attackMax}");
            GUILayout.Label($"SPD: {weapon.speed}/{weapon.speedMax}");
            GUILayout.Label($"MAG: {weapon.magic}/{weapon.magicMax}");
            GUILayout.Label($"END: {weapon.endurance}/{weapon.enduranceMax}");
        }
        GUILayout.EndArea();
    }
}
