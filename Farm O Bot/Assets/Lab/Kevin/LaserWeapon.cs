using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class LaserWeapon : NetworkBehaviour
{
    public RaycastHit hit;
    public string weaponName;
    public GameObject associatedCross;
    public LineRenderer myLaser;
    public GameObject aimObject;
    public bool canHit = true;

    [Header("Weapon")]
    public Transform startingPoint;

    public int weaponDamages = 1;
    public float weaponFireRate = 0.5f;
    public float weaponRange = 10;

    [Header("CrossHit")]
    public float scaleForce = 1;
    public float scaleSpeed = 1;

    void Start()
    {
        myLaser.gameObject.SetActive(false);
    }

    public void Shoot()
    {
        myLaser.gameObject.SetActive(true);
            
    }
    private void StopShoot()
    {
        myLaser.gameObject.SetActive(false);
    }

    void Update()
    {
        if(myLaser.gameObject.activeSelf == false)
        {
            return;
        }

        myLaser.SetPosition(0, startingPoint.position);
        myLaser.SetPosition(1, aimObject.transform.position);
        if (canHit)
        {
            Vector3 aimDirection = (aimObject.transform.position - startingPoint.position).normalized;
            if (Physics.Raycast(startingPoint.position, aimDirection, weaponRange) && hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<EnemySysteme>().TakeDamage(weaponDamages);
                StartCoroutine(weaponCooldown());
            }
        }
    }

    IEnumerator weaponCooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(weaponFireRate);
        canHit = true;
    }

    [ServerRpc]
    public void RpcShoot()
    {
        RpcClientShoot();
    }

    [ObserversRpc]
    private void RpcClientShoot()
    {
        Shoot();
    }

}
