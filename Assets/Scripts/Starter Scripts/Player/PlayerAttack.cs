using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.Rendering.Universal;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

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

	private bool canAttack = true;


	private void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		playerMoveScript = GetComponent<PlayerMovement>();
		playerAudio = GetComponent<PlayerAudio>();

		for (int i = 0; i < 10; i ++) {
			itemList.Add(null);
		}
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

		if (Input.GetKey(KeyCode.Mouse0) && weapon && weapon.itemType == ItemType.Weapon)
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
		//This is where the weapon is rotated in the right direction that you are facing
		if (weapon && canAttack)
		{
			Debug.Log("Kablammo!");
			if (UsePlayerAttackAnimations)
            {
				Debug.Log("using player attack animation");
				playerMoveScript.TriggerPlayerAttackAnimation();

			}

			if (weapon.throwable)
				weapon.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection(), rb.linearVelocity);
			else
				weapon.WeaponStart(this.transform, playerMoveScript.GetLastLookDirection());

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
			weapon.gameObject.SetActive(true);
		}

	}


	public void AddItem(GameObject prefab) {
		Debug.Log("Adding Item With Pos: " + prefab.transform.position);
		GameObject item = Instantiate(prefab, player.transform.Find("HandPosition").transform.position + prefab.transform.position, prefab.transform.rotation, player.transform);
		item.SetActive(false);

		Item weapon = item.GetComponent<Item>();

		int index = GetOpenIndex();
		itemList[index] = weapon;	
		}

	public void RemoveItem(int index) {
		if (index > 0 && index < itemList.Count && itemList[index]) {
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
		
		if (weapon != null && weapon.itemName == itemList[index].itemName) {
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
		int index = -1;

		for (int i = 0; i < itemList.Count; i++) {
			if (itemList[i] == null) {
				index = i;
				break;
			}
		}

		return index;
	}
}
