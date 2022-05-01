using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralEnemy : MonoBehaviour
{
    public NavMeshAgent[] agentsArray;

    private void Start()
    {
        /*foreach (NavMeshAgent agent in agentsArray)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(GameManager.instance.playerTransform.position, path);
        }*/
    }

    private void Update()
    {
        foreach (NavMeshAgent agent in agentsArray)
        {
            if (agent.hasPath == false)
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(GameManager.instance.playerTransform.position, path);
                agent.path = path;
                Debug.Log(agent.hasPath);
            }
        }


        bool pathPending = false;

        foreach (NavMeshAgent agent in agentsArray)
        {
            
            if (agent.pathPending)
            {
                Debug.Log("pathPending");
                pathPending = true;
                break;
            }
            else
            {
                agent.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }

        if (!pathPending)
        {
            foreach (NavMeshAgent agent in agentsArray)
            {
                agent.enabled = true;
            }
        }
    }

}
