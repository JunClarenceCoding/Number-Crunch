using System;
using UnityEngine;

public class MonsterHealthEndless : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public event Action OnMonsterDefeated;
    private Animator animator;
    void Start()
    {
        InitializeHealth();
        animator = GetComponent<Animator>();
    }
    public void InitializeHealth()
    {
        currentHealth = maxHealth;
        Debug.Log("Monster health initialized: " + currentHealth); 
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("isHit");
            }
        }
    }
    public void MonsterAttack()
    {
        if (animator != null)
            {
                animator.SetTrigger("isAttack");
            }
    }
    private void Die()
    {
        Debug.Log("Monster defeated!");
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }
        OnMonsterDefeated?.Invoke();
        Destroy(gameObject, 2f);
    }
}