using UnityEngine;

[CreateAssetMenu]
public class Apple : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();

    public enum StatToChange
    {
        Health
    };
}
