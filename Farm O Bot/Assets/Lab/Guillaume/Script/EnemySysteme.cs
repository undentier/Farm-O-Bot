using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySysteme : MonoBehaviour
{
    public NavMeshAgent selfAgent;
    public float hp;


    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
