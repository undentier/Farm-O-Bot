using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

public class PlayerFire : NetworkBehaviour
{
    public List<GlobalWeapon> leftWeaponsList;
    public List<GlobalWeapon> rightWeaponsList;

    public InputActionReference leftFireInput;
    public InputActionReference rightFireInput;
    private PlayerInput _playerInput;

    private int weaponLeftIndex = 0;
    private int weaponRightIndex = 0;

    private MechaAiming aimScript;

    private void Start()
    {
        aimScript = GetComponent<MechaAiming>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["SwitchWeaponLeft"].started += SwitchWeaponLeft;
        _playerInput.actions["SwitchWeaponLeft"].canceled += SwitchWeaponLeft;
        _playerInput.actions["SwitchWeaponRight"].started += SwitchWeaponRight;
        _playerInput.actions["SwitchWeaponRight"].canceled += SwitchWeaponRight;
    }

    private void Update()
    {
        DetectShooting();
    }

    private void SwitchWeaponLeft(InputAction.CallbackContext context)
    {
        if (context.started && IsOwner)
        {
            if (weaponLeftIndex < leftWeaponsList.Count - 1) weaponLeftIndex++;
            else weaponLeftIndex = 0;
        }
    }

    private void SwitchWeaponRight(InputAction.CallbackContext context)
    {
        if (context.started && IsOwner)
        {
            if (weaponRightIndex < rightWeaponsList.Count - 1) weaponRightIndex++;
            else weaponRightIndex = 0;
        }
    }

    private void DetectShooting()
    {
        if (leftFireInput.action.phase == InputActionPhase.Performed && IsOwner)
        {
            leftWeaponsList[weaponLeftIndex].RpcShoot(true, aimScript.aimPoint.position);
        }
        if (leftFireInput.action.phase == InputActionPhase.Waiting && IsOwner)
        {
            leftWeaponsList[weaponLeftIndex].RpcShoot(false, aimScript.aimPoint.position);
        }

        if (rightFireInput.action.phase == InputActionPhase.Performed && IsOwner)
        {
            rightWeaponsList[weaponRightIndex].RpcShoot(true, aimScript.aimPoint.position);
        }
        if (rightFireInput.action.phase == InputActionPhase.Waiting && IsOwner)
        {
            rightWeaponsList[weaponRightIndex].RpcShoot(false, aimScript.aimPoint.position);
        }
    }
}
