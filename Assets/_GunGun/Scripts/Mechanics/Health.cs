using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public event Action OnDeath;  

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");
        OnDeath?.Invoke(); 
        Destroy(gameObject);
    }

    public float gethealthFraction()
    {
        return currentHealth / maxHealth;
    }
}
