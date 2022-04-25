using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechaControllerMovement : MonoBehaviour
{
    public float speed;
    private Vector2 inputMovement;

    //References
    private Rigidbody rb;
    private DefaultInputActions playerActions;
    private Camera cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        playerActions = new DefaultInputActions();
        playerActions.Player.Enable();
    }

    private void Update()
    {
        inputMovement = playerActions.Player.Move.ReadValue<Vector2>();

        MechaMovement();
    }

    private void MechaMovement()
    {
        Vector3 direction = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;

        if (direction.magnitude >= 0.2f)
        {
            //Mecha moves in the direction he's facing
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            //Mecha movement
            Vector3 directionForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = directionForward.normalized * speed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
