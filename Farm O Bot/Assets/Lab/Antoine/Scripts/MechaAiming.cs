using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechaAiming : MonoBehaviour
{
    public LayerMask aimColliderLayer;

    [Header("Debug")]
    public bool displayDebug = false;
    public float distanceIfNoCollision;
    public Transform debugColliderPoint;

    private void Update()
    {
        Debug();
        Reticule();
    }

    private void Reticule()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayer))
        {
            if(displayDebug) debugColliderPoint.position = hit.point;
        }
        else
        {
            if(displayDebug) debugColliderPoint.position = Camera.main.ScreenToWorldPoint(new Vector3(screenCenterPoint.x, screenCenterPoint.y, distanceIfNoCollision));
        } 
    }

    private void Debug()
    {
        if(displayDebug) debugColliderPoint.gameObject.SetActive(true);
        else debugColliderPoint.gameObject.SetActive(false);
    }
}
