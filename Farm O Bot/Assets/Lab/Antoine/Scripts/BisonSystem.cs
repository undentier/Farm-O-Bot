using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FishNet.Object;

public class BisonSystem : NetworkBehaviour
{
    #region Variable

    [Header("Stats")]
    public float hp;
    private float moveFrequence;
    private float moveTimer = 0;
    private float herdRadius;
    private Vector3 herdCenter;
    private bool herdIsMoving;
    private bool mechaIsWhistling = false;

    /*[Header("Feedback")]
    public GameObject deathParticleObj;*/

    [Header("AI")]
    public NavMeshAgent selfAgent;
    public float cooldown;

    [HideInInspector] public Vector3 target;

    private NavMeshPath pathToFollow;
    private float actualCooldown = 0;

    [HideInInspector] public HerdMovement herdScript;

    #endregion

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        herdScript = GameObject.Find("Herd").GetComponent<HerdMovement>();
        moveFrequence = herdScript.bisonMoveFrequence;
        herdRadius = herdScript.herdRadius;
        herdCenter = herdScript.transform.position;

        RandomMoveInsideCercle();
        RefreshPath();
    }


    private void Update()
    {
        if (IsServer && !IsClientOnly)
        {
            herdCenter = herdScript.transform.position;
            herdIsMoving = herdScript.herddIsMoving;
            mechaIsWhistling = herdScript.mechaIsWhistling;

            if (!herdIsMoving && !mechaIsWhistling)
            {
                if (moveTimer > moveFrequence)
                {
                    moveTimer = 0;

                    RandomMoveInsideCercle();
                    RefreshPath();
                }
                else
                {
                    moveTimer += Time.deltaTime;
                }
            }
            else if (herdIsMoving)
            {
                target = herdScript.nextZone.position;
                moveTimer = moveFrequence;
            }
            else if (mechaIsWhistling)
            {
                target = herdScript.mechaWhistlingPosition.position;
                moveTimer = moveFrequence;

            }

            if (actualCooldown > cooldown)
            {
                actualCooldown = 0f;
                RefreshPath();
            }
            else
            {
                actualCooldown += Time.deltaTime;
            }
        }
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(1);
        }
    }


    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;

        if (hp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        /*GameObject particle = Instantiate(deathParticleObj, transform.position, transform.rotation);
        Destroy(particle, 5f);*/
        Destroy(gameObject);
    }

    private void RandomMoveInsideCercle()
    {
        target = herdCenter + new Vector3(Random.insideUnitSphere.x * herdRadius, herdCenter.y, Random.insideUnitSphere.z * herdRadius);
    }

    private void RefreshPath()
    {
        pathToFollow = new NavMeshPath();
        selfAgent.CalculatePath(target, pathToFollow);
        selfAgent.path = pathToFollow;
    }
}
