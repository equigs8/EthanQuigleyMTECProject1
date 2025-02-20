using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Castle : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
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
}
