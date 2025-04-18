using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

	[Header("Enemy Weapon")]
	[Tooltip("This is the current weapon that the enemy is using")]
	public Item weapon;

	[Header("Parameters")]

	public bool showChaseRadius = false;
	public bool showAttackRadius = false;
	public float chaseRadius = 3f;
	public float attackRadius = 2f;
	[Tooltip("The coolDown before you can attack again")]
	public float coolDown = 0.5f;
	private bool canAttack = true;
	private bool isAttacking = false;
	private bool sceneRunning = true;

	private Animator anim;

	private HealthBar healthBar;

    private void Start()
    {
		healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
    }

    private void Update()
	{
		anim = GetComponent<Animator>();
		sceneRunning = healthBar.maxHealth >= healthBar.currentHealth && healthBar.currentHealth > 0;

		if (anim.GetBool("isDead")) // Jane Apostol Fall '23
		{
			canAttack = false;
		}

		bool shouldAttack = false;
		Collider2D[] attackColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
		foreach (Collider2D other in attackColliders)
		{
			shouldAttack = (shouldAttack || canAttack && other.CompareTag("Player")) && sceneRunning;

			if (shouldAttack)
				Attack(other.transform.position - this.transform.position);
		}


		bool shouldChase = false;
		Collider2D[] chaseColliders = Physics2D.OverlapCircleAll(transform.position, chaseRadius);
		foreach (Collider2D other in chaseColliders)
		{
			shouldChase = (shouldChase || other.CompareTag("Player")) && sceneRunning;
		}

		anim.SetBool("isChasing", shouldChase && !shouldAttack);

	}

	public void Attack(Vector2 attackDir)
	{
		isAttacking = true;
		//This is where the weapon is rotated in the right direction that you are facing
		if (weapon && canAttack)
		{
			anim.SetBool("isAttacking", isAttacking);
			Debug.Log("Attacking Player!");

			if(healthBar != null)
			{
				healthBar.TakeDamage(weapon.healthValue);
			}
			canAttack = false;
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

	private IEnumerator CoolDown()
	{
		// anim.SetBool("isChasing", false);
		yield return new WaitForSeconds(0.2f);
		anim.SetBool("isAttacking", false);
		yield return new WaitForSeconds(coolDown);
		canAttack = true;
	}


	private void OnDrawGizmos()
	{
		if (showAttackRadius)
			Gizmos.DrawWireSphere(transform.position, attackRadius);
		if (showChaseRadius)
			Gizmos.DrawWireSphere(transform.position, chaseRadius);
	}
}
