using UnityEngine;
using TMPro;

//I used this source for using Text Mesh Pro: https://learn.unity.com/tutorial/working-with-textmesh-pro#5f86410eedbc2a00249a4928
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
    
	public enum Alignment
	{
		Player,
		Enemy,
		Environment
	}

    public int ID;
    public string itemName;
    public float imageScale = 2.5f;

	[Tooltip("This determines whose side the damager is on. If the player is wielding the weapon, then its alignment is Player")]
	public Alignment alignmnent = Alignment.Player;
	public ItemType itemType;
	// public WeaponType weaponType = WeaponType.Melee;
	public int healthValue = 0;
	public bool isStackable = false;
	public int itemQuantity = 1;
	public virtual void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection) { }
	public virtual void WeaponStart(Transform wielderPosition, Vector2 lastLookDirection, Vector2 currentVelocity) { }

	public virtual void WeaponFinished() { }

    public AudioClip placeInWorldSound;
    public AudioClip useSound;
    public AudioClip pickupSound;
	public TMP_Text quantityText;
	public bool throwable = false;
    //public int duration;

	void Update()
	{
		//If there are more than 1 item, then the quantity is displayed
		if(quantityText != null && itemQuantity > 1)
		{
			quantityText.text = "x" + itemQuantity;
		}
		//Otherwise the amount is hidden
		else if (quantityText != null)
		{
			quantityText.text = "";
		}
	}
}