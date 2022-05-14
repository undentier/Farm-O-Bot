using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSystem : MonoBehaviour
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
        if (context.started)
        {
            turretIsPreview = !turretIsPreview;

            if(turretIsPreview) SelectTurretPreview(true);
            else SelectTurretPreview(false);
        }
    }

    private void PlaceTurret(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlaceTurretInWorld();
        }
    }

    private void Update()
    {
        distanceAimPoint = Vector3.Distance(aimPosition.position, transform.position);

        UpdateTurretPreview();
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
                GameObject turretInstance = Instantiate(turret, turretPreviewInstance.transform.position, turretPreviewInstance.transform.rotation);
                currentTurretsNumber.Add(turretInstance);
            }
            else
            {
                for (int i = 0; i < currentTurretsNumber.Count; i++)
                {
                    Destroy(currentTurretsNumber[i]);
                }
                currentTurretsNumber.Clear();

                GameObject turretInstance = Instantiate(turret, turretPreviewInstance.transform.position, turretPreviewInstance.transform.rotation);
                currentTurretsNumber.Add(turretInstance);
            }
            
            Destroy(turretPreviewInstance);
            turretIsPreview = false;
        }
    }
}
