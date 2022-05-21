using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class LaserWeapon : GlobalWeapon
{
    public float dispersionRate;
    private float dispersionRateTimer;
    public float damagesRate;
    private float damagesTimer;

    [Header("Laser")]
    [Range(0, 2)]
    public float laserWidth;
    public Gradient laserColor;

    private LineRenderer laserRenderer;

    private Vector3 randomDispersionDirection;
    public List<Vector3> previousLaserDirection;

    public override void OnStartClient()
    {
        base.OnStartClient();

        SetLaserComponent();
        damagesTimer = damagesRate;
        previousLaserDirection.Add(Vector3.zero);
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
        if (isShooting && !coolingDown)
        {
            isFire = true;
            aimDirection = (LaserSpread(aimPoint) - startingPoint.position).normalized;

            DisplayLaser(aimPoint);
            LaserCast(aimDirection);
            AddEnergy();

            //Place Vibration
            _gamepadVibration.StartVibration(leftVibrationIntensity, rightVibrationIntensity);
        }
        else
        {
            if (isFire)
            {
                _gamepadVibration.StopVibration();
                isFire = false;
            }
            laserRenderer.enabled = false;
            dispersionRateTimer = 0;
            previousLaserDirection[0] = (Vector3.zero);
            if (previousLaserDirection.Count > 1) previousLaserDirection.RemoveAt(0);
        }
    }

    private void DisplayLaser(Vector3 aimPoint)
    {
        if (Vector3.Distance(aimPoint, startingPoint.position) < weaponRange)
        {
            laserRenderer.SetPosition(0, startingPoint.position);
            laserRenderer.enabled = true;
            laserRenderer.SetPosition(1, LaserSpread(aimPoint));
        }
        else
        {
            laserRenderer.enabled = false;
        }
    }

    private Vector3 LaserSpread(Vector3 aimPoint)
    {
        if (dispersionRateTimer < dispersionRate)
        {
            dispersionRateTimer += Time.deltaTime;
        }
        else
        {
            dispersionRateTimer = 0;

            float randomX = Random.Range(-weaponDispersion * 2f, weaponDispersion * 2f);
            float randomY = Random.Range(-weaponDispersion * 2f, weaponDispersion * 2f);
            randomDispersionDirection = new Vector3(randomX, randomY, 0);
            previousLaserDirection.Add(randomDispersionDirection);
            if (previousLaserDirection.Count > 2) previousLaserDirection.RemoveAt(0);
        }

        Vector3 laserDirection = aimPoint + randomDispersionDirection;
        Vector3 nextLaserTarget = Vector3.Lerp(aimPoint + previousLaserDirection[0], laserDirection, dispersionRateTimer / dispersionRate);
        return (nextLaserTarget);
    }


    private void LaserCast(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(startingPoint.position, dir, out hit, weaponRange) && (damagesTimer >= damagesRate))
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
