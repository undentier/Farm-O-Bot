using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class LaserWeapon : GlobalWeapon
{
    public float damagesRate;
    private float damagesTimer;

    [Header("Laser")]
    [Range(0, 2)]
    public float laserWidth;
    public Gradient laserColor;

    private LineRenderer laserRenderer;

    private void Start()
    {
        SetLaserComponent();
        damagesTimer = damagesRate;
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
            Vector3 aimDirection = (aimPoint - startingPoint.position).normalized;

            DisplayLaser(aimPoint);
            LaserCast(aimDirection);
        }
        else
        {
            laserRenderer.enabled = false;
        }
    }

    private void DisplayLaser(Vector3 aimPoint)
    {
        if (Vector3.Distance(aimPoint, startingPoint.position) < weaponRange)
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

    private void LaserCast(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(startingPoint.position, dir, out hit, weaponRange) && canShoot && (damagesTimer >= damagesRate))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemySysteme>().TakeDamage(weaponDamages);
                damagesTimer = 0;
            }
        }

        if(damagesTimer < damagesRate)
        {
            damagesTimer += Time.deltaTime;
        }
    }
}
