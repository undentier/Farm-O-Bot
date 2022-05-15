using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
public class ManagerMob : NetworkBehaviour
{
    [Header("R?f?rences")]
    [SerializeField]
    private GameObject mobToSpawn;
    [SerializeField]
    private float movementField;

    [Header("Mobs param?tres")]
    [SerializeField]
    [Min(0)]
    private int numbToSpawn;

    [Header("Inspector Debug")]
    [SerializeField]
    private bool isDebuging;

    //-------------//
    [Header("Spawn Setup")]
    [SerializeField] private FlocksLogic flockUnitPrefab;
    [SerializeField] private int flockSize;
    [SerializeField] private Vector3 spawnBounds;

    [Header("Speed Setup")]
    [Range(0, 10)]
    [SerializeField] private float _minSpeed;
    public float minSpeed { get { return _minSpeed; } }
    [Range(0, 10)]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get { return _maxSpeed; } }


    [Header("Detection Distances")]

    [Range(0, 10)]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance { get { return _avoidanceDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _aligementDistance;
    public float aligementDistance { get { return _aligementDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _obstacleDistance;
    public float obstacleDistance { get { return _obstacleDistance; } }

    [Range(0, 100)]
    [SerializeField] private float _boundsDistance;
    public float boundsDistance { get { return _boundsDistance; } }


    [Header("Behaviour Weights")]

    [Range(0, 10)]
    [SerializeField] private float _cohesionWeight;
    public float cohesionWeight { get { return _cohesionWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceWeight;
    public float avoidanceWeight { get { return _avoidanceWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _aligementWeight;
    public float aligementWeight { get { return _aligementWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _boundsWeight;
    public float boundsWeight { get { return _boundsWeight; } }

    [Range(0, 100)]
    [SerializeField] private float _obstacleWeight;
    public float obstacleWeight { get { return _obstacleWeight; } }

    //-------------//


    //Private parameter
    //[HideInInspector]
    public FlocksLogic[] allMobs;

    public Vector3 distanceWithGround;

    public ComputeShader computeShader;


    // Start is called before the first frame update
    void Start()
    {
        allMobs = new FlocksLogic[numbToSpawn];

        for (int i = 0; i < numbToSpawn; i++)
        {
            allMobs[i] = Instantiate(mobToSpawn, transform).GetComponent<FlocksLogic>();
            allMobs[i].gameObject.SetActive(false);
        }
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        //EnableMob();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerRpcEnableMob()
    {
        ObserverRpcSpawnMob();
    }

    [ObserversRpc]
    public void ObserverRpcSpawnMob()
    {
        SpawnMob(numbToSpawn);
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

    public void SpawnMob(int _numbTopSpawn)
    {
        for (int i = 0; i < _numbTopSpawn; i++)
        {
            allMobs[i].transform.position = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward * Random.Range(0f, movementField) + transform.position + distanceWithGround + new Vector3(0, 0.5f, 0);
            allMobs[i].AssignFlock(this);
            allMobs[i].InitializeSpeed(10f);
            allMobs[i].gameObject.SetActive(true);
        }
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

    //Compute shader test
    private void RunCompute()
    {
        int kernelHandle = computeShader.FindKernel("CSMain");

    }
}
