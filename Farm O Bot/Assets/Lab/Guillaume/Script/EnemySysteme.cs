using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySysteme : MonoBehaviour
{
    public NavMeshAgent selfAgent;

    private void Update()
    {
        selfAgent.SetDestination(GameManager.instance.playerPos.position);
    }
}
