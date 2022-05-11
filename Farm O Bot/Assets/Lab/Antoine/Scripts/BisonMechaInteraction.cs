using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BisonMechaInteraction : MonoBehaviour
{
    private PlayerInput playerInputScript;
    public HerdMovement herdScript;
    public float whistlingTime;
    private float whistlingTimer = 0;

    private void Start()
    {
        playerInputScript = GetComponent<PlayerInput>();
        playerInputScript.actions["Whistle"].started += WhistleBisons;
        playerInputScript.actions["Whistle"].canceled += WhistleBisons;
    }

    public void WhistleBisons(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            whistlingTimer = whistlingTime;
            herdScript.mechaWhistlingPosition = transform;
        }
    }

    public void Update()
    {
        if (whistlingTimer > 0)
        {
            whistlingTimer -= Time.deltaTime;
            herdScript.mechaIsWhistling = true;
        }
        else
        {
            herdScript.mechaIsWhistling = false;
        }
    }
}
