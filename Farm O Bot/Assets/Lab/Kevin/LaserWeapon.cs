using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class LaserWeapon : GlobalWeapon
{
    [Header("Laser")]
    private LineRenderer laserRenderer;
    [Range(0, 2)]
    public float laserWidth;
    public Gradient laserColor;

    private RaycastHit hit;

    private bool canHit = false;

    private void Start()
    {
        SetLaserComponent();
    }

    private void SetLaserComponent()
    {
        laserRenderer = gameObject.AddComponent<LineRenderer>();
        laserRenderer.enabled = false;
        laserRenderer.useWorldSpace = true;
        laserRenderer.startWidth = laserWidth;
        laserRenderer.colorGradient = laserColor;
    }

    public override void Shoot(bool isShooting, Vector3 aimPoint)
    {
        if (isShooting)
        {
            laserRenderer.SetPosition(0, startingPoint.position);
            laserRenderer.enabled = true;
            laserRenderer.SetPosition(1, aimPoint);
        }
        else
        {
            laserRenderer.enabled = false;
        }
    }

    /*void Update()
    {

        laserRenderer.SetPosition(0, startingPoint.position);
        laserRenderer.SetPosition(1, aimObject.transform.position);
        if (canHit)
        {
            Vector3 aimDirection = (aimObject.transform.position - startingPoint.position).normalized;
            if (Physics.Raycast(startingPoint.position, aimDirection, weaponRange) && hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<EnemySysteme>().TakeDamage(weaponDamages);
                StartCoroutine(weaponCooldown());
            }
        }
    }*/

    /*IEnumerator weaponCooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(weaponFireRate);
        canHit = true;
    }*/
}
