using UnityEngine;

public enum ItemType
{
    Consumable,
    Material,
    Weapon,
    Trap,
    Firewood
}

public class Item : MonoBehaviour
{
    public int ID;
    public string itemName;
    public ItemType itemType;
    public float imageScale = 2.5f;
    public int healAmount = 0;
    public int damageAmoumt = 0;
    public AudioClip placeInWorldSound;
    public AudioClip useSound;
    public AudioClip pickupSound;
    //public int duration;
}