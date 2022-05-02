using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    public CanvasManager canvas;

    public List<GlobalWeapon> leftWeapons;
    public List<GlobalWeapon> rightWeapons;

    public InputActionReference leftFire;
    public InputActionReference rightFire;

    private void Update()
    {
        if (leftFire.action.phase == InputActionPhase.Performed)
        {
            FireLeft();
        }
        if (rightFire.action.phase == InputActionPhase.Performed)
        {
            FireRight();
        }
    }


    public void FireLeft()
    {
        for (int i = 0; i < leftWeapons.Count; i++)
        {
            leftWeapons[i].Shoot(canvas);
        }    
    }
    public void FireRight()
    {
        for (int i = 0; i < rightWeapons.Count; i++)
        {
            rightWeapons[i].Shoot(canvas);          
        }
    }
}
