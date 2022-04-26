using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    public List<GlobalWeapon> leftWeapons;
    public List<GlobalWeapon> rightWeapons;

    public InputAction leftFire;
    public InputAction rightFire;

    private void Update()
    {
        Debug.Log(leftFire.phase);
        if (leftFire.phase == InputActionPhase.Disabled)
        {
            FireLeft();
        }

    }


    public void FireLeft()
    {
        for (int i = 0; i < leftWeapons.Count; i++)
        {
            leftWeapons[i].Shoot();
        }    
    }
    public void FireRight()
    {
        for (int i = 0; i < rightWeapons.Count; i++)
        {
            rightWeapons[i].Shoot();
        }
    }
}
