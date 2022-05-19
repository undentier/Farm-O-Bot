using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class IgnoreParentRotation : NetworkBehaviour
{
    Quaternion selfRotation;

    private void Awake()
    {
        selfRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (IsServer && !IsClientOnly) transform.rotation = selfRotation;
    }
}
