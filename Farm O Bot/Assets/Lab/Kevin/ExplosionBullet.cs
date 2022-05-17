using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : GlobalBullet
{
    public GameObject explosion;

    protected virtual void OnDestroy()
    {
        GameObject explos = Instantiate(explosion, transform.position, explosion.transform.rotation);
    }
}
