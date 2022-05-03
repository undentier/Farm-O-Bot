using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralEnemy : MonoBehaviour
{
    public NavMeshAgent[] agentsArray;

    private void Start()
    {
        foreach (NavMeshAgent agent in agentsArray)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(GameManager.instance.playerTransform.position, path);
            agent.path = path;
        }
    }

    private void Update()
    {
        /*foreach (NavMeshAgent agent in agentsArray)
        {
            if (agent.hasPath == false)
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(GameManager.instance.playerTransform.position, path);
                agent.path = path;
            }
        }*/
    }

}
