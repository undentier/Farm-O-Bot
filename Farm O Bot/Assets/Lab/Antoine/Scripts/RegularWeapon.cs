using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularWeapon : GlobalWeapon
{
    public float weaponFireRate;
    public int bulletsPerShoot;
    public float timeBtwShots;
    private int bulletsShot;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletSpeed;

    [Space] public bool FireAlwaysPressed = false;

    public override void Shoot(bool isShooting, Vector3 aimPoint)
    {
        if (isShooting && canShoot && !coolingDown)
        {
            canShoot = false;
            isFire = true;
            aimDirection = (aimPoint - startingPoint.position).normalized;

            bulletsShot = bulletsPerShoot;
            ShootOneBullet();
            base.AddEnergy();
            Invoke("ResetShot", weaponFireRate);

            //Place Camera Shake
            //Place Vibration
            //Place MuzzleFlash
        }
        else
        {
            if(!FireAlwaysPressed) isFire = false;
        }
    }

    private void ShootOneBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, startingPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));

        bullet.GetComponent<GlobalBullet>().maxDistance = weaponRange;
        bullet.GetComponent<GlobalBullet>().originPoint = startingPoint;
        bullet.GetComponent<GlobalBullet>().bulletDamage = weaponDamages;

        bullet.GetComponent<Rigidbody>().velocity = (BulletSpread(bullet.transform) * (bulletSpeed * 500) * Time.deltaTime);

        bulletsShot--;
        if (bulletsShot > 0)
        {
            Invoke("ShootOneBullet", timeBtwShots);
        }
    }

    Vector3 BulletSpread(Transform originalDirection)
    {
        float randomX = Random.Range(-weaponDispersion * 0.1f, weaponDispersion * 0.1f);
        float randomY = Random.Range(-weaponDispersion * 0.1f, weaponDispersion * 0.1f);
        Vector3 randomDispersionDirection = new Vector3(randomX, randomY, 0);

        Vector3 bulletDirection = originalDirection.forward + randomDispersionDirection;
        return bulletDirection;
    }

    private void ResetShot()
    {
        canShoot = true;
        if (FireAlwaysPressed) isFire = false;
    }
}
