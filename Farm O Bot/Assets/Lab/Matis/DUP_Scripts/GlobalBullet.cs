using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBullet : MonoBehaviour
{
    public float maxDistance;
    public Transform originPoint;

    Vector3 distanceFromOrigin;

    private void Update()
    {
        distanceFromOrigin = (transform.position - originPoint.position);

        if (maxDistance < distanceFromOrigin.magnitude)
        {
            Destroy(gameObject);
        }
    }
}
