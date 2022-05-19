using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerStart : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.playerTransformList.Add(gameObject.transform);
    }
}
