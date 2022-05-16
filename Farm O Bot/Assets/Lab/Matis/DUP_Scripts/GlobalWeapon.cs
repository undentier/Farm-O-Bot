using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class GlobalWeapon : NetworkBehaviour
{
    public string weaponName;
    public GameObject associatedCross;

    [Header("Weapon")]
    public Transform startingPoint;

    public int weaponDamages = 1;
    public float weaponFireRate = 0.5f;
    public float weaponRange = 10;
    public float weaponDispersion = 0;

    [Header("CrossHit")]
    public float scaleForce = 1;
    public float scaleSpeed = 1;


    [Header("Bullet")]
    public GameObject bulletPrefab;

    public float bulletSpeed;

    bool canShoot = true;

    private void Shoot(Vector3 aimPoint)
    {
        if (canShoot == true)
        {
            Vector3 aimDirection = (aimPoint - startingPoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, startingPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));

            bullet.GetComponent<GlobalBullet>().maxDistance = weaponRange;
            bullet.GetComponent<GlobalBullet>().originPoint = startingPoint;

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
        //spread = startingPoint.rotation * spread;

        return spread;
    }


    IEnumerator weaponCooldown()
    {
        yield return new WaitForSeconds(weaponFireRate);
        canShoot = true;
    }

    [ServerRpc]
    public void RpcShoot(Vector3 aimPoint)
    {
        RpcClientShoot(aimPoint);
    }

    [ObserversRpc]
    private void RpcClientShoot(Vector3 aimPoint)
    {
        Shoot(aimPoint);
    }
}
