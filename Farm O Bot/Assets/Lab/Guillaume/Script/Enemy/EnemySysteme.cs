using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FishNet.Object;

public class EnemySysteme : NetworkBehaviour
{
    #region Variable

    [Header ("Stats")]
    public float hp;
    public float rangePlayerAggro;

    [Header("Feedback")]
    public GameObject deathParticleObj;

    [Header ("AI")]
    public NavMeshAgent selfAgent;
    public float cooldown;

    [HideInInspector]public Transform target;

    private NavMeshPath pathToFollow;
    private float actualCooldown;

    #endregion

    private void Start()
    {
        target = GameManager.instance.playerTransform;
        actualCooldown = 0f;
    }

    private void Update()
    {
        if (actualCooldown > cooldown)
        {
            selfAgent.SetDestination(target.position);
            actualCooldown = 0f;
            //RefreshPath();
        }
        else
        {
            actualCooldown += Time.deltaTime;
        }

        PlayerDetection();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }


    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;

        if (hp <= 0)
        {
            Die();
        }
    }

    [ObserversRpc]
    private void Die()
    {
        GameObject particle = Instantiate(deathParticleObj, transform.position, transform.rotation);
        Destroy(particle, 5f);
        Destroy(gameObject);
    }


    private void PlayerDetection()
    {
        
    }

    private void RefreshPath()
    {
        pathToFollow = new NavMeshPath();
        selfAgent.CalculatePath(target.position, pathToFollow);
        selfAgent.path = pathToFollow;
    }

}
