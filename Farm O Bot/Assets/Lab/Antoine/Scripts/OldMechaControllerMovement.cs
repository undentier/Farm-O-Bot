using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class OldMechaControllerMovement : MonoBehaviour
{
    public float speed;
    public float turnTime;
    private float turnVelocity;

    private Vector2 inputMovement;
    private Vector2 inputLook;

    //References
    private Rigidbody rb;
    private DefaultInputActions playerActions;
    private Camera cam;
    public CinemachineFreeLook Cinecam;
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
        inputLook = playerActions.Player.Look.ReadValue<Vector2>();

        MechaMovement();
        MechaLook();
    }

    private void MechaMovement()
    {
        Vector3 direction = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;

        if (direction.magnitude >= 0.2f)
        {
            //Mecha moves in the cam direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg /*+ cam.transform.eulerAngles.y*/;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Mecha movement
            Vector3 directionForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = directionForward.normalized * speed * Time.deltaTime;
            mechaAnimationScript.WalkAnimation(true);
            //Cinecam.m_RecenterToTargetHeading.m_enabled = true;
        }
        else
        {
            rb.velocity = Vector3.zero;
            mechaAnimationScript.WalkAnimation(false);
        }
    }

    private void MechaLook()
    {
        Vector3 look = new Vector3(inputLook.x, 0f, inputLook.y).normalized;

        if (look.magnitude > 0f)
        {
            //Cinecam.m_RecenterToTargetHeading.m_enabled = false;
        }
    }
}
