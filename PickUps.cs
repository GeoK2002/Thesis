/*The game has 3 types of collectibles. Coins(which end the game), MedKits (that heal the player) and knifes (that refill the amount of projectiles the player has left)*/
/*All 3 scripts are in this file in order:*/
/*  1.Coin 2.MedKit 3.Knife  */
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject canvasUI; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HandleCoinCollection());
        }
    }

    private IEnumerator HandleCoinCollection()
    {
        gameObject.SetActive(false);

        Time.timeScale = 0f;

        if (canvasUI != null)
        {
            canvasUI.SetActive(true);
        }

        yield return null;
    }
}






using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitPickUp : MonoBehaviour
{
    public HealthManager healthManager; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(healthManager.healthAmount==100)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            healthManager.Heal(100);
            gameObject.SetActive(false); 
        }
    }
}







using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public PlayerAttack playerAttack; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(playerAttack.currentProjectiles==5)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            RefillProjectiles();
            gameObject.SetActive(false); 
        }
    }

    private void RefillProjectiles()
    {
        if (playerAttack == null)
        {
            return;
        }

        if(playerAttack.currentProjectiles==playerAttack.maxProjectiles)
        {
            return;
        }

        if(playerAttack.currentProjectiles<playerAttack.maxProjectiles)
        {
        playerAttack.currentProjectiles = playerAttack.maxProjectiles;
        playerAttack.UpdateProjectileCounterUI();
        }
    }
}

