using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

public class PlayerFire : NetworkBehaviour
{
    public List<GlobalWeapon> leftWeapons;
    public List<GlobalWeapon> rightWeapons;

    public InputActionReference leftFireInput;
    public InputActionReference rightFireInput;

    private MechaAiming aimScript;

    private void Start()
    {
        aimScript = GetComponent<MechaAiming>();
    }

    private void Update()
    {
        if (leftFireInput.action.phase == InputActionPhase.Performed && IsOwner)
        {
            FireLeft(true);
        }
        if (leftFireInput.action.phase == InputActionPhase.Waiting && IsOwner)
        {
            FireLeft(false);
        }

        if (rightFireInput.action.phase == InputActionPhase.Performed && IsOwner)
        {
            FireRight(true);
        }
        if (rightFireInput.action.phase == InputActionPhase.Waiting && IsOwner)
        {
            FireRight(false);
        }
    }

    public void FireLeft(bool isShooting)
    {
        /*for (int i = 0; i < leftWeapons.Count; i++)
        {
            leftWeapons[i].RpcShoot(aimScript.aimPoint.position);
        }*/

        leftWeapons[0].RpcShoot(isShooting, aimScript.aimPoint.position);
    }
    public void FireRight(bool isShooting)
    {
        /*for (int i = 0; i < rightWeapons.Count; i++)
        {
            rightWeapons[i].RpcShoot(aimScript.aimPoint.position);          
        }*/

        rightWeapons[0].RpcShoot(isShooting, aimScript.aimPoint.position);
    }
}
