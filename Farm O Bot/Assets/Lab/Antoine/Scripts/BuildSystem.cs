using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet;
using FishNet.Connection;

public class BuildSystem : NetworkBehaviour
{
    [Header("Turrets")]
    public int maxNumberTurretsPlaced;
    [HideInInspector] public List<GameObject> currentTurretsNumber;
    private bool turretIsPreview = false;
    public float maxDistanceBuild;
    private float distanceAimPoint;

    public GameObject turret;
    public GameObject turretPreview;
    private GameObject turretPreviewInstance;

    [Space]
    public Transform aimPosition;
    public Transform chest;
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
        if (context.started && IsOwner)
        {
            turretIsPreview = !turretIsPreview;

            if(turretIsPreview) SelectTurretPreview(true);
            else SelectTurretPreview(false);
        }
    }

    private void PlaceTurret(InputAction.CallbackContext context)
    {
        if (context.started && IsOwner)
        {
            PlaceTurretInWorld();
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            distanceAimPoint = Vector3.Distance(aimPosition.position, transform.position);

            UpdateTurretPreview();
        }
    }

    private void SelectTurretPreview(bool enable)
    {
        switch (enable)
        {
            case true:
                turretPreviewInstance = Instantiate(turretPreview, aimPosition.position, Quaternion.identity);

                break;
            case false:
                Destroy(turretPreviewInstance);
                break;
        }
    }

    private void UpdateTurretPreview()
    {
        if (turretIsPreview)
        {
            if (distanceAimPoint < maxDistanceBuild)
            {
                turretPreviewInstance.SetActive(true);
                turretPreviewInstance.transform.position = aimPosition.position;
                turretPreviewInstance.transform.rotation = Quaternion.Euler(0, chest.rotation.eulerAngles.y, 0);
            }
            else
            {
                turretPreviewInstance.SetActive(false);
            }
        }
    }

    private void PlaceTurretInWorld()
    {
        if (turretIsPreview && distanceAimPoint < maxDistanceBuild)
        {
            if (currentTurretsNumber.Count < maxNumberTurretsPlaced)
            {
                SpawnTurretServeur(turretPreviewInstance.transform.position, turretPreviewInstance.transform.rotation);
            }
            else
            {
                DespawnTurretServeur(currentTurretsNumber);
                currentTurretsNumber.Clear();

                SpawnTurretServeur(turretPreviewInstance.transform.position, turretPreviewInstance.transform.rotation);
            }
            
            Destroy(turretPreviewInstance);
            turretIsPreview = false;
        }
    }
    
    [ServerRpc]
    private void SpawnTurretServeur(Vector3 turretPreviewPos, Quaternion turretPreviewRot)
    {
        GameObject turretInstance = Instantiate(turret, turretPreviewPos, turretPreviewRot);
        InstanceFinder.ServerManager.Spawn(turretInstance, Owner);
        AddTurretToList(Owner, turretInstance);
    }

    [TargetRpc]
    private void AddTurretToList(NetworkConnection conection, GameObject turretObj)
    {
        currentTurretsNumber.Add(turretObj);
    }

    [ServerRpc]
    private void DespawnTurretServeur(List<GameObject> turrets)
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            InstanceFinder.ServerManager.Despawn(turrets[i]);
        }
    }
}
