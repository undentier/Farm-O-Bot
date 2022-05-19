using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreParentRotation : MonoBehaviour
{
    Quaternion selfRotation;

    private void Start()
    {
        selfRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = selfRotation;
    }
}
