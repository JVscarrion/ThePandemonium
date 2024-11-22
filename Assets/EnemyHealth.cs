using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject deathEffect;
    public GameObject deathEffect2;
    void Start()
    {      
        currentHealth = maxHealth;
       
    }
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        // On Hit Coins
        CoinManager.instance.AddCoins(1);
      
        if (currentHealth <= 0)
        {
            Die();
            // on Die Coins
            CoinManager.instance.AddCoins(5);      
        }
    }

    void Die()
    {      
        
        GameObject vfxSpawned = Instantiate(deathEffect, transform.position, Quaternion.identity);
        GameObject vfxSpawned2 = Instantiate(deathEffect2, transform.position, deathEffect2.transform.rotation);
        Destroy(vfxSpawned, 3);
        Destroy(vfxSpawned2, 3);
        Destroy(gameObject);
    }
}
