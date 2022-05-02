using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalWeapon : MonoBehaviour
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


    public void Shoot(CanvasManager _canvas)
    {
        if (canShoot == true)
        {
            GameObject bullet = Instantiate(bulletPrefab, startingPoint.position, startingPoint.rotation);
            _canvas.CrossSpreadScale(associatedCross, scaleForce, scaleSpeed);

            bullet.GetComponent<GlobalBullet>().maxDistance = weaponRange;
            bullet.GetComponent<GlobalBullet>().originPoint = startingPoint;

            Vector3 bulletWay = BulletSpread(startingPoint);

            bullet.GetComponent<Rigidbody>().velocity = (bulletWay * (bulletSpeed*500) * Time.deltaTime);

            canShoot = false;

            StartCoroutine(weaponCooldown());
        }
    }

    Vector3 BulletSpread(Transform originPoint)
    {
        Vector3 spread = originPoint.forward;

        float deviation = Random.Range(0, weaponDispersion);
        float angle = Random.Range(0, 360f);

        spread = Quaternion.AngleAxis(deviation, originPoint.up) * originPoint.forward;
        spread = Quaternion.AngleAxis(angle, originPoint.forward) * spread;
        //spread = startingPoint.rotation * spread;

        return spread;
    }


    IEnumerator weaponCooldown()
    {
        yield return new WaitForSeconds(weaponFireRate);
        canShoot = true;
    }
}
