using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class MechaControllerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    //public float rotationSpeedLegs;
    public float turnTime;
    private float turnVelocity;
    public Transform legs;
    private float legsRotationX;

    [Header("Aim")]
    public float rotationSpeedChest;
    public bool clampChestRotationHorizontal = false;
    public Vector2 clampAngleHorizontal;
    public bool clampChestRotationVertical = false;
    public Vector2 clampAngleVertical;
    private float chestRotationX;
    private float chestRotationY;
    public Transform chest;

    private Vector2 inputMovement;
    private Vector2 inputLook;

    //References
    private Rigidbody rb;
    private DefaultInputActions playerActions;
    private MechaAnimation mechaAnimationScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mechaAnimationScript = GetComponent<MechaAnimation>();
        playerActions = new DefaultInputActions();
        playerActions.Player.Enable();
    }

    private void Update()
    {
        ReadInput();

        RotateMechaDownBody();
        RotateMechaUpBody();
        MoveMecha();
    }

    private void ReadInput()
    {
        inputMovement = playerActions.Player.Move.ReadValue<Vector2>();
        inputLook = playerActions.Player.Look.ReadValue<Vector2>();
    }

    private void RotateMechaDownBody()
    {
        //legsRotationX += inputMovement.x * rotationSpeedLegs * Time.deltaTime;

        //legs.rotation = Quaternion.Euler(legs.rotation.eulerAngles.x, legsRotationX + 180f, legs.rotation.eulerAngles.z);
    }

    private void RotateMechaUpBody()
    {
        chestRotationX += inputLook.x * rotationSpeedChest * Time.deltaTime;
        chestRotationY += -inputLook.y * rotationSpeedChest * Time.deltaTime;

        if (clampChestRotationHorizontal) chestRotationX = Mathf.Clamp(chestRotationX, clampAngleHorizontal.x, clampAngleHorizontal.y);
        if (clampChestRotationVertical) chestRotationY = Mathf.Clamp(chestRotationY, clampAngleVertical.x, clampAngleVertical.y);

        //chest.rotation = Quaternion.Euler(chestRotationY, legsRotationX + chestRotationX, 0);
        chest.rotation = Quaternion.Euler(chestRotationY, chestRotationX, 0);
    }

    private void MoveMecha()
    {
        /*Vector3 movement = -chest.forward * -inputMovement.y;

        if (movement.magnitude > 0.3f) 
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
            mechaAnimationScript.WalkAnimation(true);
        }
        else
        {
            mechaAnimationScript.WalkAnimation(false);
        }*/

        Vector3 movementDirection = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;

        if (movementDirection.magnitude >= 0.2f)
        {
            //Rotate smoothly to direction
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + chest.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Mecha movement
            Vector3 directionForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(rb.position + directionForward.normalized * speed * Time.deltaTime);
            mechaAnimationScript.WalkAnimation(true);
        }
        else mechaAnimationScript.WalkAnimation(false);
    }
}
