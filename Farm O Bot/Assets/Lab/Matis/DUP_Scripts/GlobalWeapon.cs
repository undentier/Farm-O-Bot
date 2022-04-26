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
        if (canShoot)
        {
            //GameObject bullet = Instantiate(bulletPrefab, startingPoint.position, startingPoint.rotation);
            Debug.Log(name + "FIRE");
            canShoot = false;

            StartCoroutine(weaponCooldown());
        }
    }


    IEnumerator weaponCooldown()
    {
        yield return new WaitForSeconds(weaponFireRate * Time.deltaTime);
        canShoot = true;
    }
}
