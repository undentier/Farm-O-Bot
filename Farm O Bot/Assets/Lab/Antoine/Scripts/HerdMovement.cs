using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HerdMovement : MonoBehaviour
{
    [Header("Bison")]
    public GameObject bison;
    public Transform bisonsGroup;
    public float bisonMoveFrequence;

    [Header("Herd")]
    public Transform zones;
    public int numberBisons;
    public float herdRadius;
    public float herdMovingSpeed;
    public float herdMoveFrequence;
    private float herdMoveTimer = 0;

    private Transform[] zonesPosition;
    private float[] zonesDistance;
    private float[] ZonesDistanceOrdered;

    [Space]
    public Transform[] nearestZones;
    private Transform lastZone;
    [HideInInspector] public Transform nextZone;
    [HideInInspector] public Transform mechaWhistlingPosition;

    private int x = 0;
    [HideInInspector] public bool herddIsMoving = false;
    [HideInInspector] public bool mechaIsWhistling = false;

    [Header("Debug")]
    public bool debugHerdRadius = false;

    private void Start()
    {
        for (int i = 0; i < numberBisons; i++)
        {
            GameObject bisonInstance = Instantiate(bison, transform.position + new Vector3(Random.insideUnitSphere.x * herdRadius, transform.position.y, Random.insideUnitSphere.z * herdRadius), bison.transform.rotation, bisonsGroup);
            bisonInstance.GetComponent<BisonSystem>().herdScript = this;
        }

        zonesPosition = new Transform[zones.childCount];
        zonesDistance = new float[zones.childCount];
        ZonesDistanceOrdered = zonesDistance;
        nearestZones = new Transform[2];

        for (int i = 0; i < zones.childCount; i++)
        {
            zonesPosition[i] = zones.GetChild(i);
        }
    }

    private void Update()
    {
        if (herdMoveTimer > herdMoveFrequence && !herddIsMoving)
        {
            RefreshZonesDistance();
            SortNearestZones();
            ChooseNextZone();
            herddIsMoving = true;
        }
        else
        {
            herdMoveTimer += Time.deltaTime;
            x = 0;
        }

        if (herddIsMoving) MoveHerd();
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
        ZonesDistanceOrdered = zonesDistance;
        ZonesDistanceOrdered = ZonesDistanceOrdered.OrderBy(x => x).ToArray();

        for (int i = 0; i < nearestZones.Length; i++)
        {
            int index = System.Array.IndexOf(zonesDistance, ZonesDistanceOrdered[i+1]);

            if (zonesPosition[index] == lastZone)
            {
                x++;
            }

            nearestZones[i] = zonesPosition[System.Array.IndexOf(zonesDistance, ZonesDistanceOrdered[i + 1 + x])];
        }
    }

    private void ChooseNextZone()
    {
        int randomZone = Random.Range(0, 2);

        lastZone = zonesPosition[System.Array.IndexOf(zonesDistance, ZonesDistanceOrdered[0])];
        nextZone = nearestZones[randomZone];
    }

    private void MoveHerd()
    {
        if (Vector3.Distance(transform.position, nextZone.position) > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextZone.position, Time.deltaTime * herdMovingSpeed);
        }
        else
        {
            herddIsMoving = false;
            herdMoveTimer = 0;
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
