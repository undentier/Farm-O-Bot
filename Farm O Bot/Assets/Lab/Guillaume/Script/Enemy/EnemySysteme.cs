using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySysteme : MonoBehaviour
{
    #region Variable

    [Header ("Stats")]
    public float hp;

    [Header("Feedback")]
    public GameObject deathParticleObj;

    [Header ("AI")]
    public NavMeshAgent selfAgent;
    public float cooldown;

    [HideInInspector]public Transform target;

    private NavMeshPath pathToFollow;
    private float actualCooldown;

    #endregion

    private void Start()
    {
        target = GameManager.instance.playerTransform;
        actualCooldown = 0f;

        RefreshPath();
    }

    private void Update()
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
        //selfAgent.SetDestination(target.position);
    }


    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player Bullet")
        {
            //TakeDamage(Récupérer les dégats)
        }
    }
    */


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
        GameObject particle = Instantiate(deathParticleObj, transform.position, transform.rotation);
        Destroy(particle, 5f);
        Destroy(gameObject);
    }



    private void RefreshPath()
    {
        pathToFollow = new NavMeshPath();
        selfAgent.CalculatePath(target.position, pathToFollow);
        selfAgent.path = pathToFollow;
    }

}
