using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Turret : NetworkBehaviour
{
    private GameObject actualTarget;
    public Transform firePoint;
    public float detectionRange = 15f;
    public float headTurnSpeed = 5f;
    public float fireRate = 1f;

    private float leftTime = 0f;

    [Header("Bullet")]
    public GameObject bullet;
    public int bulletDamage = 1; //not functionnal
    public string enemyTag = "Enemy";

    [Header("Head")]
    public Transform head;
    public Animator headAnimator;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    void UpdateTarget()
    {
        GameObject[] myEnnemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float distanceMin = Mathf.Infinity;
        GameObject target = null;

        foreach(GameObject enemy in myEnnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < distanceMin)
            {
                distanceMin = distance;
                target = enemy;
            }
        }

        if(target != null && distanceMin <= detectionRange)
        {
            actualTarget = target;
        }
        else
        {
            actualTarget = null;
        }
    }

    void Update()
    {
        if(actualTarget == null)
        {
            return;
        }

        Vector3 dirrection = actualTarget.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dirrection);
        Vector3 myRotation = Quaternion.Lerp(head.rotation, rotation, Time.deltaTime * headTurnSpeed).eulerAngles;
        head.rotation = Quaternion.Euler(0f, myRotation.y, 0f);

        if (leftTime <= 0)
        {
            Shoot();
            leftTime = 1 / fireRate;
        }

        leftTime -= Time.deltaTime;
    }
    void Shoot()
    {
        headAnimator.SetTrigger("shoot");
        GameObject bulletInstance = Instantiate(bullet, firePoint.position, firePoint.rotation);
        TurretBullet theBullet = bulletInstance.GetComponent<TurretBullet>();

        if(theBullet != null)
        {
            theBullet.Seek(actualTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
