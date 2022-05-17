using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : GlobalBullet
{

    public Vector3 explosionCercleRange;
    public float timeBeforeExplose;
    public float explosionDamage;

    protected void Start()
    {
        StartCoroutine(LerpScale(transform.localScale, explosionCercleRange, timeBeforeExplose));

    }

    /*protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy")
        {
            collider.GetComponent<EnemySysteme>().TakeDamage(explosionDamage);
        }
    }*/

    IEnumerator LerpScale(Vector3 startScale, Vector3 endScale, float lerpTime)
    {
        float startTime = Time.time;
        float endTime = startTime + lerpTime;

        while (Time.time < endTime)
        {
            float timeProgressed = (Time.time - startTime) / lerpTime;  
            transform.localScale = Vector3.Lerp(startScale, endScale, timeProgressed);

            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }

    /*protected override void Update()
    {
        
    }*/

    protected void OnDestroy()
    {

    }
}
