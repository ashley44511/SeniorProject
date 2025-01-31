using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
public class DamageOverTime : MonoBehaviour
{
    public int DamageToPlayer = 40;
    public float TimeToDamage = 2f;
    public HealthBar playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Damage over time");
            playerHealth.TakeDamageOverTime(DamageToPlayer, TimeToDamage);
            Destroy(gameObject);
            Debug.Log(playerHealth.currentHealth);
        }
    }
}
