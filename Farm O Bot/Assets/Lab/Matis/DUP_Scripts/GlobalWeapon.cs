using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;

public class GlobalWeapon : NetworkBehaviour
{
    public string weaponName;
    public Transform startingPoint;

    [Header("Energy")]
    public float energyLosedRate;

    [Header("Weapon")]
    public int weaponDamages;
    public float weaponRange;

    [HideInInspector] public bool canShoot = true;

    private EnergySystem _energySystem;

    public virtual void Start()
    {
        _energySystem = GetComponent<EnergySystem>();
    }

    public virtual void Shoot(bool isShooting, Vector3 aimPoint)
    {
        //Place weapon behavior here
    }

    public virtual void RemoveEnergy()
    {
        //_energySystem.RemoveEnergy(energyLosedRate);
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
