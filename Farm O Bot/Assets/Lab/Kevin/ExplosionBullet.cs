using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : GlobalBullet
{
    [Header("Explosion Settings")]
    public float explosionSize;
    public float timeBeforeExplose;
    public Material explosiveMaterial;
    private bool flagOnce = false;

    public override void Update()
    {
        base.Update();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!flagOnce)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponentInChildren<MeshRenderer>().material = explosiveMaterial;
            StartCoroutine(LerpScale(transform.localScale, new Vector3(explosionSize, explosionSize, explosionSize), timeBeforeExplose));
            flagOnce = true;
        }

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemySysteme>().TakeDamage(bulletDamage);
        }
    }

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
}
