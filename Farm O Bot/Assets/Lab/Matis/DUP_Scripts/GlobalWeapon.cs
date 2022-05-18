using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

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

    [HideInInspector] public EnergySystem _energySystem;

    private bool energyFulled = false;

    public override void OnStartClient()
    {
        base.OnStartClient();

        _energySystem = GetComponentInParent<EnergySystem>();
    }

    private void Update()
    {
        if (IsOwner && (_energySystem.currentEnergy < _energySystem.maxEnergy))
        {
            energyFulled = false;
        }
        if (IsOwner && (_energySystem.currentEnergy >= _energySystem.maxEnergy))
        {
            energyFulled = true;
        }
    }

    public virtual void Shoot(bool isShooting, Vector3 aimPoint)
    {
        //Place weapon behavior here
    }

    public virtual void AddEnergy()
    {
        if (IsOwner)
        {
            _energySystem.AddEnergy(energyLosedRate);
        }
    }

    [ServerRpc]
    public void RpcShoot(bool isShooting, Vector3 aimPoint)
    {
        RpcClientShoot(isShooting, aimPoint, energyFulled);
    }

    [ObserversRpc]
    private void RpcClientShoot(bool isShooting, Vector3 aimPoint, bool energyIsFull)
    {
        if (!energyIsFull) Shoot(isShooting, aimPoint);
        else Shoot(false, aimPoint);
    }
}
