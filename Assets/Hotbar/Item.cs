using UnityEngine;

public enum ItemType
{
    Consumable,
    Material,
    Weapon,
    Trap
}

public class Item : MonoBehaviour
{
    public int ID;
    public string itemName;
    public ItemType itemType;
}
