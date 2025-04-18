using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
	[Header("References")]

	[Tooltip("There are separate animators for the weapon. If you have a player attack animation from a sprite sheet, make this true. If your attack animation is the weapon itself moving (separate from the player) this can be false.")]
	public bool UsePlayerAttackAnimations = false;
	public Animator anim;
	public Rigidbody2D rb;
	public GameObject player;
	public PlayerMovement playerMoveScript;


	[Header("Player Weapons")]
	[Tooltip("This is the list of all the weapons that your player uses")]
	public List<Item> itemList;
	[Tooltip("This is the current weapon that the player is using")]
	public Item weapon;
	[Tooltip("The coolDown before you can attack again")]
	public float coolDown = 0.4f;

	[Header("Audio")]
	public PlayerAudio playerAudio;

	private bool canAttack;
	private HealthBar healthBar;


	private void Start()
	{
		while (itemList.Count < 10) {
			itemList.Add(null);
		}

		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		playerMoveScript = GetComponent<PlayerMovement>();
		playerAudio = GetComponent<PlayerAudio>();
		healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
		canAttack = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))//Here is where you can hit the "1" key on your keyboard to activate this weapon
		{
			if (itemList.Count > 0)
			{
				SwitchWeaponAtIndex(0);
			}
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))//Remove this if you don't have multiple weapons
		{
			if (itemList.Count > 1)
			{
				SwitchWeaponAtIndex(1);
			}
		}

		if (Input.GetKey(KeyCode.Mouse0) && weapon && weapon.itemType == ItemType.Weapon && healthBar.currentHealth > 0)
		{
			Attack();
			if (playerAudio && !playerAudio.AttackSource.isPlaying && playerAudio.AttackSource.clip != null)
			{
				playerAudio.AttackSource.Play();
			}
		}
		else
		{
			StopAttack();
		}
	}

	public void Attack()
	{
		//Null weapon check
		if (weapon == null)
		{
			Debug.Log("Inventory slot empty");
			return;
		}

		//This is where the weapon is rotated in the right direction that you are facing		
		if (weapon && canAttack)
		{
			Debug.Log("Kablammo!");
			if (UsePlayerAttackAnimations)
            {
				Debug.Log("using player attack animation");
				playerMoveScript.TriggerPlayerAttackAnimation();

			}

			if (weapon.throwable && (PlayerHasArrows() >= 0))
			{
				FireArrow();
			}
			else if (!weapon.throwable)
			{
				if(this.transform != null && playerMoveScript != null)
				{
					weapon.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection());
				}
			}
			StartCoroutine(CoolDown());
		}
	}

	public void StopAttack()
	{
		if (weapon)
		{
			weapon.WeaponFinished();
		}
	}

	public void SwitchWeaponAtIndex(int index)
	{
		if (weapon)
		{
			weapon.gameObject.SetActive(false);
		}


		//Makes sure that if the index exists, then a switch will occur
		if (index < itemList.Count && itemList[index] != null)
		{
			//Switch weapon to index then activate
			weapon = itemList[index];

			// Only set active if in a night scene
			if (IsNightScene())
			{
				Debug.Log("Weapon at index " + index + " is activated");
				weapon.gameObject.SetActive(true);
			}
		}

	}


	public void AddItem(GameObject prefab) {
		int index = GetOpenIndex();
		if (index >= 0) {
			GameObject item = Instantiate(prefab, player.transform.Find("HandPosition").transform.position + prefab.transform.position, prefab.transform.rotation, player.transform);
			item.SetActive(false);

			Item weapon = item.GetComponent<Item>();

			itemList[index] = weapon;	
		} else {
			Debug.Log("COULDN'T FIND OPEN INVENTORY SLOT FOR ITEM " + weapon);
		}
		}

	public void RemoveItem(int index) {
		if (index >= 0 && index < itemList.Count && itemList[index]) {
			GameObject item = itemList[index].gameObject;
			item.SetActive(false);
			itemList[index] = null;
			Destroy(item);
		}
	}
		
	public void RemoveItem(GameObject prefab) {
		int index = GetItemIndex(prefab);
		RemoveItem(index);
	}

	public void RemoveItem(Item item) {
		int index = GetItemIndex(item);
		RemoveItem(index);
	}

	public void MoveItem(GameObject prefab, int moveTo) {
		Debug.Log("Swapping Items to" + moveTo);
		int index = GetItemIndex(prefab);


    	(itemList[index], itemList[moveTo]) = (itemList[moveTo], itemList[index]);
		
		if (weapon != null && itemList[index] != null && weapon.itemName == itemList[index].itemName) {
			weapon = itemList[moveTo];
			itemList[index].gameObject.SetActive(false);

			if (weapon != null) {
				weapon.gameObject.SetActive(true);
			}

		} else {
			weapon = itemList[index];
			itemList[moveTo].gameObject.SetActive(false);

			if (weapon != null) {
				weapon.gameObject.SetActive(true);
			}
		}


    }

		public void MoveItem(GameObject prefab, GameObject moveTo) {
		int index = GetItemIndex(moveTo);

		MoveItem(prefab, index);
    }
	

	private IEnumerator CoolDown()
	{
		canAttack = false;
		yield return new WaitForSeconds(coolDown);
		canAttack = true;
	}

	private int GetItemIndex(Item weapon) {

		int index = -1;
		
		if (weapon == null) {
			return index;
		}
	
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList[i] != null && itemList[i].itemName == weapon.itemName) {
				index = i;
				break;
			}
		}

		return index;
	}

		private int GetItemIndex(GameObject prefab) {
		GameObject tempItem = Instantiate(prefab);
		tempItem.SetActive(false);
		Item weapon = tempItem.GetComponent<Item>();

		int index = GetItemIndex(weapon);
		
		Destroy(tempItem);

		return index;
	}

	private int GetOpenIndex() {
		while (itemList.Count < 10) {
			itemList.Add(null);
		}

		int index = -1;
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList[i] == null) {
				index = i;
				break;
			}
		}

		return index;
	}
	private bool IsNightScene()
	{
		string sceneName = SceneManager.GetActiveScene().name;
		return sceneName.Contains("Night");
	}

	private int PlayerHasArrows()
	{
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList[i] != null && itemList[i].name == "Arrow") {
				return i;
			}
		}

		return -1;
	}

	private bool FireArrow() 
	{
		int arrowIndex = PlayerHasArrows();

		if (arrowIndex == -1) 
			return false;

		Debug.Log("Firing Arrow");

		weapon.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection(), rb.linearVelocity);
		
		itemList[arrowIndex].itemQuantity -= 1;
		if (itemList[arrowIndex].itemQuantity == 0) 
		{
			itemList[arrowIndex] = null;
		}

		return true;
	}

	private void ClearInventory() {
		for (int i = 0; i < 10; i++) {
			itemList[i] = null;
			weapon = null;
		}
	}
}
