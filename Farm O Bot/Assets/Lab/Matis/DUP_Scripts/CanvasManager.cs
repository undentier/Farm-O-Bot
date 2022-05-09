using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public void CrossSpreadScale(GameObject cross, float force, float speed)
    {
        Debug.Log(cross.transform.localScale);

        if(cross.transform.localScale.x < force+1 && cross.transform.localScale.y < force+1)
        {
            Vector3 variation = new Vector3(speed, speed, 0);
            cross.transform.localScale += variation;

            //CrossSpreadReduce(cross);
        }
        else
        {
            Vector3 variation = new Vector3(speed, speed, 0);
            cross.transform.localScale -= variation;
        }
    }

    void CrossSpreadReduce(GameObject cross)
    {
        Vector3 reference = new Vector3(1, 1, 1);
        Vector3 reduction = new Vector3(-0.1f, -0.1f, 0);


        while(cross.transform.localScale != reference)
        {
            cross.transform.localScale += reduction;
        }
    }
}
