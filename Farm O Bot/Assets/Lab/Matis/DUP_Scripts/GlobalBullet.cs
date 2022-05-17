using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBullet : MonoBehaviour
{
    //variables filled with weapon script : 
    [HideInInspector] public float maxDistance;
    [HideInInspector] public Transform originPoint;
    [HideInInspector] public float bulletDamage;

    Vector3 distanceFromOrigin;

    public virtual void Update()
    {
        distanceFromOrigin = (transform.position - originPoint.position);

        if (maxDistance < distanceFromOrigin.magnitude)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemySysteme>().TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
