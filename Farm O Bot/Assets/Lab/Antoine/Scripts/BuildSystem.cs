using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSystem : MonoBehaviour
{
    public int maxNumberTurretsPlaced;
    private int currentNumberTurrets;
    private bool turretVisualisation = false;

    private PlayerInput playerInputScript;
    
    private void Start()
    {
        playerInputScript = GetComponent<PlayerInput>();
        playerInputScript.actions["SelectTurret"].started += SelectTurret;
        playerInputScript.actions["SelectTurret"].canceled += SelectTurret;

        playerInputScript.actions["PlaceTurret"].started += PlaceTurret;
        playerInputScript.actions["PlaceTurret"].canceled += PlaceTurret;
    }

    private void SelectTurret(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            turretVisualisation = !turretVisualisation;
        }
    }

    private void PlaceTurret(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlaceTurretInWorld();
        }
    }

    private void PlaceTurretInWorld()
    {
        if (turretVisualisation && currentNumberTurrets < maxNumberTurretsPlaced)
        {
            
            currentNumberTurrets++;
        }
    }
}
