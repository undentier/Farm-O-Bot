using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalWeapon : MonoBehaviour
{
    public string name;


    [Header("Weapon")]
    public Transform startingPoint;

    public int weaponDamages = 1;
    public float weaponFireRate = 0.5f;
    public float weaponRange = 10;


    [Header("Bullet")]
    public GameObject bulletPrefab;

    public float bulletSpeed;



    bool canShoot = true;


    public void Shoot()
    {
        if (canShoot == true)
        {
            GameObject bullet = Instantiate(bulletPrefab, startingPoint.position, startingPoint.rotation);

            bullet.GetComponent<GlobalBullet>().maxDistance = weaponRange;
            bullet.GetComponent<GlobalBullet>().originPoint = startingPoint;
            bullet.GetComponent<Rigidbody>().velocity = (startingPoint.forward * (bulletSpeed*500) * Time.deltaTime);

            canShoot = false;

            StartCoroutine(weaponCooldown());
        }
    }


    IEnumerator weaponCooldown()
    {
        yield return new WaitForSeconds(weaponFireRate);
        canShoot = true;
    }
}
