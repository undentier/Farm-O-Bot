using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Variable

    [Header ("Stats")]
    public float spawnRate;
    public int numOfEnemySpawnRate;
    public int maxEnemyToSpawn;
    public float speed;

    [Header ("Enemy wich spawn")]
    public GameObject wichEnemy;

    [Header ("Each spawn point")]
    public Transform[] spawnPoints;

    [HideInInspector]public Transform target;

    private float actualTimer;
    private int numOfEnemySpawn;
    private bool isGrounded;

    #endregion

    private void Update()
    {
        if (isGrounded == true)
        {
            SpawnerSysteme();
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
                Instantiate(wichEnemy, spawnPoints[random].position, spawnPoints[random].rotation);

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
            Destroy(gameObject);
        }
    }

    private void MoveTowardTarget()
    {
        if (Vector3.Distance(transform.position, target.position) > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        }
        else
        {
            isGrounded = true;
        }
    }
}
