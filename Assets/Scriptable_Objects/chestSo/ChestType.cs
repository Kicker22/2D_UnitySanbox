using UnityEngine;

[CreateAssetMenu(menuName = "Game/Chest Type")]
public class ChestType : ScriptableObject
{
    public string chestName;
    public Sprite closedSprite;
    public Sprite openedSprite;
    public int goldAmount;
}