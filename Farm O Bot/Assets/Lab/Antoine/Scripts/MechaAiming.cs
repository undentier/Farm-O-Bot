using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechaAiming : MonoBehaviour
{
    public LayerMask aimColliderLayer;
    public float distanceIfNoCollision;
    public float aimAdjustTime;
    private Vector3 velocity;

    [Header("Debug")]
    public bool displayDebug = false;
    public Transform colliderPoint;
    public Transform aimPoint;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Debug();
        Reticule();
        AdjustAim();
    }

    private void Reticule()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayer)) colliderPoint.position = hit.point;
        else colliderPoint.position = Camera.main.ScreenToWorldPoint(new Vector3(screenCenterPoint.x, screenCenterPoint.y, distanceIfNoCollision));
    }

    private void AdjustAim()
    {
        aimPoint.position = Vector3.SmoothDamp(aimPoint.position, colliderPoint.position, ref velocity, aimAdjustTime); aimPoint.gameObject.SetActive(true);
    }

    private void Debug()
    {
        if(displayDebug) colliderPoint.gameObject.SetActive(true); 
        else colliderPoint.gameObject.SetActive(false); aimPoint.gameObject.SetActive(false);
    }
}
