using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Castle : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;

    public CastleHealthBar healthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            DestroyCastle();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
    }
    void DestroyCastle()
    {
        Destroy(gameObject);
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
