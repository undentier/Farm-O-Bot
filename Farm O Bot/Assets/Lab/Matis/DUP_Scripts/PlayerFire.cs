using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

public class PlayerFire : NetworkBehaviour
{
    //public CanvasManager canvas;

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
            FireLeft();
        }
        if (rightFireInput.action.phase == InputActionPhase.Performed && IsOwner)
        {
            FireRight();
        }
    }


    public void FireLeft()
    {
        for (int i = 0; i < leftWeapons.Count; i++)
        {
            leftWeapons[i].RpcShoot(aimScript.aimPoint.position);
        }    
    }
    public void FireRight()
    {
        for (int i = 0; i < rightWeapons.Count; i++)
        {
            rightWeapons[i].RpcShoot(aimScript.aimPoint.position);          
        }
    }
}
