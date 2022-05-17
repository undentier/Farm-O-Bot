using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularWeapon : GlobalWeapon
{
    public float weaponFireRate;
    public float weaponDispersion;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletSpeed;

    public override void Shoot(bool isShooting, Vector3 aimPoint)
    {
        if (isShooting && canShoot)
        {
            Vector3 aimDirection = (aimPoint - startingPoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, startingPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));

            bullet.GetComponent<GlobalBullet>().maxDistance = weaponRange;
            bullet.GetComponent<GlobalBullet>().originPoint = startingPoint;
            bullet.GetComponent<GlobalBullet>().bulletDamage = weaponDamages;

            Vector3 bulletWay = BulletSpread(startingPoint, bullet.transform);

            bullet.GetComponent<Rigidbody>().velocity = (bulletWay * (bulletSpeed * 500) * Time.deltaTime);

            canShoot = false;

            StartCoroutine(weaponCooldown());
        }
    }

    Vector3 BulletSpread(Transform originPoint, Transform direction)
    {
        Vector3 spread = direction.forward;

        float deviation = Random.Range(0, weaponDispersion);
        float angle = Random.Range(0, 360f);

        spread = Quaternion.AngleAxis(deviation, direction.up) * direction.forward;
        spread = Quaternion.AngleAxis(angle, direction.forward) * spread;

        return spread;
    }

    IEnumerator weaponCooldown()
    {
        yield return new WaitForSeconds(weaponFireRate);
        canShoot = true;
    }
}
