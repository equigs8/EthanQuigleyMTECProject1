using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Castle : MonoBehaviour
{
    public string owner;
    public float maxHealth;
    public float currentHealth;

    public HealthBar healthBar;
    public GameManager gameManager;
    public bool isAlive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool GetIsAlive()
    {
        return isAlive;    
    }
    
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0 && isAlive)
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
        isAlive = false;
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        gameManager.removeCastle(owner, gameObject);
        Destroy(gameObject);
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
