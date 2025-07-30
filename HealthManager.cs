/*Here are included 2 simple functions for the player taking damage and healing that are very important for other scripts*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public Image healthBar;
    public float healthAmount = 100f;
    public PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;
    public DeathMenuManager deathMenuManager;

    void Start()
    {

    }

    void Update()
    {
        if(healthAmount <= 0)
        {
            deathMenuManager.ShowDeathMenu();
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthBar.fillAmount = healthAmount / 100f;
    }

}
