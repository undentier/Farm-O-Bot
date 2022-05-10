using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdMovement : MonoBehaviour
{
    public GameObject bison;
    public int numberBisons;
    public float herdRadius;
    public float bisonMoveFrequence;
    public float herdMoveFrequence;

    [Header("Debug")]
    public bool debugHerdRadius = false;

    private void Start()
    {
        for (int i = 0; i < numberBisons; i++)
        {
            Instantiate(bison, transform.position + new Vector3(Random.insideUnitSphere.x * herdRadius, transform.position.y, Random.insideUnitSphere.z * herdRadius), bison.transform.rotation, transform);
        }
    }

    private void OnDrawGizmos()
    {
        if (debugHerdRadius)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, herdRadius);
        }
    }
}
