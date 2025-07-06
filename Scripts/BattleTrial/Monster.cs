using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int MaxHealth = 500;
    public int currentHealth;

    void Start()
    {
        currentHealth = MaxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Enemy defeated");
            GetComponent<Animator>().SetBool("isDead", true);
        }
        else{
            GetComponent<Animator>().SetTrigger("isHit");
        }
    }
}