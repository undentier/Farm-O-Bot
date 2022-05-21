using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class GlobalWeapon : NetworkBehaviour
{
    public string weaponName;
    public Transform startingPoint;
    public bool leftWeapon = false;
    [HideInInspector] public Vector3 aimDirection;

    [Header("Feedback")]
    public GameObject muzzleFlash;
    public float camShakeIntensity, camShakeDuration;
    public float vibrationTime;
    public float leftJoystickVibrationSpeed;
    public float rightJoystickVibrationSpeed;

    [Header("Energy")]
    public float energyLosedRate;
    public float coolingDownTime;

    [Header("Weapon")]
    public int weaponDamages;
    public float weaponRange;
    public float weaponDispersion;

    [HideInInspector] public bool canShoot = true;
    [HideInInspector] public bool isFire = false;
    [HideInInspector] public bool coolingDown = false;
    private bool playOnce = true;

    [HideInInspector] public EnergySystem _energySystem;
    private Animator chestAnimator;

    public override void OnStartClient()
    {
        base.OnStartClient();

        _energySystem = GetComponentInParent<EnergySystem>();
        chestAnimator = transform.parent.GetChild(0).GetComponent<Animator>();
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

    public virtual void StartWeaponCooldown()
    {
        if(playOnce) StartCoroutine(weaponCooldown());
    }

    private IEnumerator weaponCooldown()
    {
        playOnce = false;
        _energySystem.isCooldown = true;
        coolingDown = true;
        yield return new WaitForSeconds(coolingDownTime);
        coolingDown = false;
        _energySystem.isCooldown = false;
        playOnce = true;
    }

    public virtual void AnimationShoot()
    {
        if (leftWeapon)
        {
            chestAnimator.SetTrigger("isShootingLeft");
        }
        else
        {
            chestAnimator.SetTrigger("isShootingRight");
        }
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
