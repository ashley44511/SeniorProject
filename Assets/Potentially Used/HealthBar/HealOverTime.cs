using UnityEngine;

public class HealOverTime : MonoBehaviour
{
    public int HealingToPlayer = 40;
    public float TimeToHeal = 2f;
    public HealthBar playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Heal Over Time");
            playerHealth.HealingOverTime(HealingToPlayer, TimeToHeal);
            Destroy(gameObject);
            Debug.Log(playerHealth.currentHealth);
        }
    }
}
