using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HerdMovement : MonoBehaviour
{
    [Header("Bison")]
    public GameObject bison;
    public float bisonMoveFrequence;

    [Header("Herd")]
    public Transform zones;
    public int numberBisons;
    public float herdRadius;
    public float herdMoveFrequence;
    private float herdMoveTimer = 0;

    private Transform[] zonesPosition;
    private float[] zonesDistance;
    private float[] ZonesDistanceOrdered;
    public Transform[] nearestZones;
    private Transform lastZone;

    [Header("Debug")]
    public bool debugHerdRadius = false;

    private void Start()
    {
        for (int i = 0; i < numberBisons; i++)
        {
            Instantiate(bison, transform.position + new Vector3(Random.insideUnitSphere.x * herdRadius, transform.position.y, Random.insideUnitSphere.z * herdRadius), bison.transform.rotation, transform);
        }

        zonesPosition = new Transform[zones.childCount];
        zonesDistance = new float[zones.childCount];
        ZonesDistanceOrdered = zonesDistance;
        nearestZones = new Transform[2];

        for (int i = 0; i < zones.childCount; i++)
        {
            zonesPosition[i] = zones.GetChild(i);
        }

        RefreshZonesDistance();
        SortNearestZones();
    }

    private void RefreshZonesDistance()
    {
        for (int i = 0; i < zonesPosition.Length; i++)
        {
            zonesDistance[i] = (Vector3.Distance(zonesPosition[i].position, transform.position));
        }
    }

    private void SortNearestZones()
    {
        ZonesDistanceOrdered = ZonesDistanceOrdered.OrderBy(x => x).ToArray();

        for (int i = 0; i < nearestZones.Length; i++)
        {
            int index = System.Array.IndexOf(zonesDistance, ZonesDistanceOrdered[i+1]);
            nearestZones[i] = zonesPosition[index];
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
