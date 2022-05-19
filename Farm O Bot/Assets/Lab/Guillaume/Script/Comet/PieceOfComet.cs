using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

public class PieceOfComet : NetworkBehaviour
{
    #region Variable

    [Header("Stats")]
    public float spawnRate;
    public int numOfEnemySpawnRate;
    public int maxEnemyToSpawn;
    public float speed;

    [Header("Wich enemy to spawn")]
    public GameObject wichEnemy;

    [Header("Each spawn point")]
    public Transform[] spawnPoints;

    [Header("Feedback")]
    public ParticleSystem trailParticle;

    [HideInInspector] public Transform target;

    private float actualTimer;
    private int numOfEnemySpawn;
    private bool isGrounded;

    [HideInInspector] public ObjectifFeedback _objectifFeedback;

    #endregion

    private void Update()
    {
        if (isGrounded == true)
        {
            if (!IsClientOnly)
            {
                SpawnerSysteme();
            }
        }
        else
        {
            MoveTowardTarget();
        }
    }

    private void SpawnerSysteme()
    {
        if (actualTimer > spawnRate)
        {
            actualTimer = 0f;

            for (int i = 0; i < numOfEnemySpawnRate; i++)
            {
                int random = Random.Range(0, spawnPoints.Length - 1);
                GameObject actualEnemy = PoolEnemyManager.instance.SpawnFromPool("Enemy", spawnPoints[random].position, spawnPoints[random].rotation);
                //GameObject actualEnemy = Instantiate(wichEnemy, spawnPoints[random].position, spawnPoints[random].rotation);
                InstanceFinder.ServerManager.Spawn(actualEnemy, Owner);

                numOfEnemySpawn += 1;
                CheckIfLimitReach();
            }
        }
        else
        {
            actualTimer += Time.deltaTime;
        }
    }
    private void CheckIfLimitReach()
    {
        if (numOfEnemySpawn >= maxEnemyToSpawn)
        {
            _objectifFeedback.DestroyAlert(this.gameObject);
            Destroy(gameObject);
        }
    }

    private void MoveTowardTarget()
    {
        if (Vector3.Distance(transform.position, target.position) > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
            transform.LookAt(target.position);
        }
        else
        {
            isGrounded = true;
            trailParticle.Stop();
        }
    }
}
