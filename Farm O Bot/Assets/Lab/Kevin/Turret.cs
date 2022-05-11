using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameObject actualTarget;
    public float detectionRange = 15f;
    public float turnSpeed = 5f;

    public string enemyTag = "Enemy";

    public float cadence = 1f;
    private float leftTime = 0f;

    public GameObject bulletPrefab;
    public int bulletDamage =1; //not functionnal
    public Transform firePoint;

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

    // Update is called once per frame
    void Update()
    {
        if(actualTarget == null)
        {
            return;
        }

        Vector3 dirrection = actualTarget.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dirrection);
        Vector3 myRotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, myRotation.y, 0f);

        if(leftTime <= 0)
        {
            Shoot();
            leftTime = 1 / cadence;
        }

        leftTime -= Time.deltaTime;
    }
    void Shoot()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet theBullet = bullet.GetComponent<Bullet>();

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
