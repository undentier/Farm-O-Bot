using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Variable

    [Header ("Stats")]
    public float spawnRate;
    public int numOfEnemy;

    [Header ("Enemy wich spawn")]
    public GameObject wichEnemy;

    [Header ("Each spawn point")]
    public Transform[] spawnPoints;

    private float actualTimer;
    private int numOfEnemySpawn;

    #endregion

    private void Update()
    {
        SpawnerSysteme();
    }

    private void SpawnerSysteme()
    {
        if (actualTimer > spawnRate)
        {
            for (int i = 0; i < numOfEnemy; i++)
            {
                int random = Random.Range(0, spawnPoints.Length - 1);
                Instantiate(wichEnemy, spawnPoints[random].position, spawnPoints[random].rotation);
                numOfEnemy += 1;

                //Debug.Log(numOfEnemy);
            }

            Debug.Log("je rentre");
            actualTimer = 0f;

        }
        else
        {
            actualTimer += Time.deltaTime;
        }
    }
}
