using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechaControllerMovement : MonoBehaviour
{
    public float speed;
    public float turnTime;
    private float turnVelocity;
    private Vector2 inputMovement;

    //References
    private Rigidbody rb;
    private DefaultInputActions playerActions;
    private Camera cam;
    private MechaAnimation mechaAnimationScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        mechaAnimationScript = GetComponent<MechaAnimation>();
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
            //Mecha moves in the cam direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Mecha movement
            Vector3 directionForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = directionForward.normalized * speed * Time.deltaTime;
            mechaAnimationScript.WalkAnimation(true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            mechaAnimationScript.WalkAnimation(false);
        }
    }
}
