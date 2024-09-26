using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            animator.SetTrigger("Die");
            Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

}
