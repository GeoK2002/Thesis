/*This script dictates the enemy's health, knockback and death*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxHealth = 100;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector2 knockback)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

}
