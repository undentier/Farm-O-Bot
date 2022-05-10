using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BisonSystem : MonoBehaviour
{
    #region Variable

    [Header("Stats")]
    public float hp;
    private float moveFrequence;
    private float moveTimer = 0;
    private float herdRadius;
    private Vector3 herdCenter;

    /*[Header("Feedback")]
    public GameObject deathParticleObj;*/

    [Header("AI")]
    public NavMeshAgent selfAgent;
    public float cooldown;

    [HideInInspector] public Vector3 target;

    private NavMeshPath pathToFollow;
    private float actualCooldown = 0;

    private HerdMovement herdScript;

    #endregion

    private void Start()
    {
        herdScript = GetComponentInParent<HerdMovement>();
        moveFrequence = herdScript.bisonMoveFrequence;
        herdRadius = herdScript.herdRadius;
    }

    private void Update()
    {
        herdCenter = herdScript.transform.position;

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

        if (target != Vector3.zero)
        {
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
