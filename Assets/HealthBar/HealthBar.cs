using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Gradient gradient;
    public Image fill;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public float damageCooldownTime = 1f;
    public Transform healthBarTransform;
    private bool canTakeDamage = true;

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
        //Implement later, can load game over scene if the player dies or go to different scene (like last checkpoint)
        if(currentHealth <= 0)
        {
            //SceneManager.LoadScene("GameOver");
        }

    }

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
        if (canTakeDamage)
        {
        canTakeDamage = false;
        currentHealth -= damage;
        healthBar.value = currentHealth;

        fill.color = gradient.Evaluate(healthBar.normalizedValue);
        StartCoroutine(CoolDown());
        }
    }

    public IEnumerator TakeDamageOverTime(int damage, float time)
    {
        yield return StartCoroutine(DamageOverTime(damage, time));
    }

    public IEnumerator DamageOverTime(int damage, float duration)
    {
        float damagePerSecond = damage / duration;
        float damageInterval = 0.1f;
        float elapsed = 0f;
        float damageAccumulator = 0f;

        while (elapsed < duration)
        {
            damageAccumulator += damagePerSecond * damageInterval;

            int wholeDamage = Mathf.FloorToInt(damageAccumulator);
            if (wholeDamage >= 1)
            {
                TakeDamage(wholeDamage);
                damageAccumulator -= wholeDamage;
            }

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


	private IEnumerator CoolDown()
	{
		yield return new WaitForSeconds(damageCooldownTime);
		canTakeDamage = true;
	}
}
