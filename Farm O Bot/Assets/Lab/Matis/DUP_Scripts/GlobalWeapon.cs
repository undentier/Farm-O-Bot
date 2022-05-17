using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class GlobalWeapon : NetworkBehaviour
{
    public string weaponName;

    [Header("Weapon")]
    public Transform startingPoint;
    public int weaponDamages;
    public float weaponRange;

    

    public virtual void Shoot(bool isShooting, Vector3 aimPoint)
    {
        //Place weapon behavior here
    }

    [ServerRpc]
    public void RpcShoot(bool isShooting, Vector3 aimPoint)
    {
        RpcClientShoot(isShooting, aimPoint);
    }

    [ObserversRpc]
    private void RpcClientShoot(bool isShooting, Vector3 aimPoint)
    {
        Shoot(isShooting, aimPoint);
    }
}
