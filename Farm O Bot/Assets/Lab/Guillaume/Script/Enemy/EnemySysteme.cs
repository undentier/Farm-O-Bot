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

    private float actualCooldown;
    private bool targetOnPlayer;
    private float startHp;

    #endregion

    private void Start()
    {
        actualCooldown = 0f;
        startHp = hp;
    }

    private void Update()
    {
        if (IsClient && IsServer)
        {
            RefreshPathSysteme();

            if (targetOnPlayer == false)
            {
                PlayerDetection();
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangePlayerAggro);
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

        PutBackEnemyInPool();
    }

    private void PutBackEnemyInPool()
    {
        if(target == null)
        {
            target = GameManager.instance.playerTransformList[0];
        }
        hp = startHp;
        actualCooldown = 0f;

        PoolEnemyManager.instance.AddObjectToPool("Enemy", gameObject);
    }
    
    [Server]
    private void PlayerDetection()
    {
        for (int i = 0; i < GameManager.instance.playerTransformList.Count; i++)
        {
            float playerDist = Vector3.Distance(transform.position, GameManager.instance.playerTransformList[i].position);
            float bisonDist = Vector3.Distance(transform.position, GameManager.instance.bisonTransform.position);

            if (playerDist < rangePlayerAggro && playerDist < bisonDist)
            {
                target = GameManager.instance.playerTransformList[i];
                selfAgent.SetDestination(target.position);

                targetOnPlayer = true;
            }
            
        }
    }

    [Server]
    private void RefreshPathSysteme()
    {
        if (actualCooldown > cooldown && target != null)
        {
            selfAgent.SetDestination(target.position);
            actualCooldown = 0f;
        }
        else
        {
            actualCooldown += Time.deltaTime;
            if(target == null)
            {
                target = GameManager.instance.playerTransformList[0];
            }
        }
    }



}
