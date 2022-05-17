using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using NaughtyAttributes;

public class ManagerMob : NetworkBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField]
    private GameObject mobToSpawn;
    [SerializeField]
    public float movementField;

    [Header("Mobs param?tres")]
    [SerializeField]
    [Min(0)]
    private int numbToSpawn;

    [Header("Inspector Debug")]
    [SerializeField]
    private bool isDebuging;

    [Header("Speed")]
    [Range(0, 100)]
    [SerializeField] private float speed;

    [Header("Detection Distances")]
    [Range(0, 10)]
    public float cohesionDistance;

    [Range(0, 100)]
    public float avoidanceDistance;

    [Range(0, 10)]
    public float aligementDistance;

    [Range(0, 10)]
    public float obstacleDistance;


    [Header("Behaviour Weights")]
    [Range(0, 10)]
    public float cohesionWeight;

    [Range(0, 10)]
    public float avoidanceWeight;

    [Range(0, 10)]
    public float aligementWeight;

    [Range(0, 10)]
    public float boundsWeight;

    [Range(0, 100)]
    public float obstacleWeight;

    [Range(0, 10)]
    public float targetWeight;


    //Private parameter
    [HideInInspector]
    public FlocksLogic[] allMobs;
    [HideInInspector]
    public Vector3 distanceWithGround;

    // Start is called before the first frame update
    void OnEnable()
    {
        allMobs = new FlocksLogic[numbToSpawn];

        for (int i = 0; i < numbToSpawn; i++)
        {
            allMobs[i] = Instantiate(mobToSpawn, null).GetComponent<FlocksLogic>();
            allMobs[i].gameObject.SetActive(false);
            allMobs[i].InitializeTarget(this.transform);
        }
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        //EnableMob();
    }

    [Button]
    [ServerRpc(RequireOwnership = false)]
    public void ServerRpcEnableMob()
    {
        Vector3[] allPosition = new Vector3[allMobs.Length];
        for (int i = 0; i < allMobs.Length; i++)
        {
            Vector3 positionToSpawn = Quaternion.Euler(Random.Range(0f, 180f), Random.Range(0f, 360f), 0) * transform.forward * Random.Range(0f, movementField) + transform.position + distanceWithGround + new Vector3(0, 0.5f, 0);
            allPosition[i] = positionToSpawn;
        }
        
        ObserverRpcSpawnMob(allPosition);
    }

    [ObserversRpc]
    public void ObserverRpcSpawnMob(Vector3[] positions)
    {
        for(int i = 0; i<allMobs.Length; i++)
        {
            SpawnMob(allMobs[i], positions[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            //distanceWithGround = hit.point - transform.position;
        }

        for (int i = 0; i < allMobs.Length; i++)
        {
            allMobs[i].MoveUnit();
        }
    }

    public void SpawnMob(FlocksLogic mob,Vector3 position)
    {
        mob.transform.position = position;
        mob.AssignFlock(this);
        mob.InitializeSpeed(10f);
        mob.gameObject.SetActive(true);
    }

    public void DestroyMob(int mobToDestroy)
    {
        allMobs[mobToDestroy].gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (isDebuging)
        {
            if (distanceWithGround != Vector3.zero)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, distanceWithGround + transform.position);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(distanceWithGround + transform.position, movementField);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, movementField);
            }
        }
    }

}
