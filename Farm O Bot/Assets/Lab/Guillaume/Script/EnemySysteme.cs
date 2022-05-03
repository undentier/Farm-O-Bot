using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySysteme : MonoBehaviour
{
    public NavMeshAgent selfAgent;
    public float hp;
    public Transform target;

    private Vector3 lastWayPoint;
    private NavMeshPath pathToFollow;

    public float cooldown;
    private float actualCooldown;

    private void Start()
    {
        target = GameManager.instance.playerTransform;
        actualCooldown = 0f;

        RefreshPath();
    }

    private void Update()
    {
        if (actualCooldown < cooldown)
        {
            actualCooldown += Time.deltaTime;
        }
        else
        {
            actualCooldown = 0f;
            RefreshPath();
        }
        //selfAgent.SetDestination(target.position);
    }


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



    private void RefreshPath()
    {
        pathToFollow = new NavMeshPath();
        selfAgent.CalculatePath(target.position, pathToFollow);
        selfAgent.path = pathToFollow;
    }

}
