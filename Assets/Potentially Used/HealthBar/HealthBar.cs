using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Gradient gradient;
    public Image fill;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public Transform healthBarTransform;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBarTransform = transform;
        SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //Implement later, can load game over scene if the player dies
        if(currentHealth <= 0)
        {
            //SceneManager.LoadScene("GameOver");
        }

    }

/*     void LateUpdate()
    {
        if (transform.parent != null && transform.parent.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    } */

    public void SetCurrentHealth(int health)
    {
        healthBar.value = health;
        currentHealth = health;
    }

    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void Heal(int health)
    {
        if(currentHealth + health > 100)
        {
            currentHealth = 100;
            healthBar.value = 100;
        }
        else
        {
            currentHealth += health;
            healthBar.value = currentHealth;
        }

        fill.color = gradient.Evaluate(healthBar.normalizedValue);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        fill.color = gradient.Evaluate(healthBar.normalizedValue);
    }

    public void TakeDamageOverTime(int damage, float time)
    {
        StartCoroutine(DamageOverTime(damage, time));
    }
    
    private IEnumerator DamageOverTime(int damage, float duration)
    {
        float damagePerSecond = damage / duration;
        float damageInterval = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            TakeDamage(Mathf.CeilToInt(damagePerSecond * damageInterval));
            elapsed += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public void HealingOverTime(int damage, float time)
    {
        StartCoroutine(HealOverTime(damage, time));
    }
    
    private IEnumerator HealOverTime(int damage, float duration)
    {
        float damagePerSecond = damage / duration;
        float damageInterval = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Heal(Mathf.CeilToInt(damagePerSecond * damageInterval));
            elapsed += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
