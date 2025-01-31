using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDamage : MonoBehaviour
{
    public int DamageToPlayer = 40;
    public HealthBar playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Instant Damage");
            playerHealth.TakeDamage(DamageToPlayer);
            Destroy(gameObject);
            Debug.Log(playerHealth.currentHealth);
        }
    }
}