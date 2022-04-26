using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public List<GlobalWeapon> leftWeapons;
    public List<GlobalWeapon> rightWeapons;


    public void FireLeft()
    {
        for(int i = 0; i < leftWeapons.Count; i++)
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
