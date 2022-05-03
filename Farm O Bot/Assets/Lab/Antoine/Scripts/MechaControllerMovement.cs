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
    public bool legMoveWithChestRotation = false;
    public float turnTime;
    private bool turnDownBody = false;
    private float targetAngleChest = 0;
    private float turnVelocity;
    public Transform legs;
    private float legsRotationX;
    private float legsBaseAngle;
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

        legsBaseAngle = (legs.eulerAngles.y - 180);
    }

    private void Update()
    {
        ReadInput();

        if(legMoveWithChestRotation) RotateMechaDownBody();
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

        if (chestRotationX >= clampAngleHorizontal.y || chestRotationX <= clampAngleHorizontal.x)
        {
            targetAngleChest = chest.eulerAngles.y -180;
            chestRotationX = 0;
            //legs.rotation = Quaternion.Euler(legs.rotation.eulerAngles.x, targetAngleChest, legs.rotation.eulerAngles.z);
            legsBaseAngle = (targetAngleChest - 180);
            turnDownBody = true;
        }

        if (turnDownBody)
        {
            float angle = Mathf.SmoothDampAngle(legs.eulerAngles.y, targetAngleChest, ref turnVelocity, turnTime);
            legs.rotation = Quaternion.Euler(legs.rotation.eulerAngles.x, angle, legs.rotation.eulerAngles.z);
        }
    }

    private void RotateMechaUpBody()
    {
        Vector3 lookDirection = new Vector2(inputLook.x, inputLook.y).normalized;

        if (lookDirection.magnitude >= 0.2f)
        {
            chestRotationX += lookDirection.x * rotationSpeedChest * Time.deltaTime;
            chestRotationY += -lookDirection.y * rotationSpeedChest * Time.deltaTime;

            if (clampChestRotationHorizontal) { chestRotationX = Mathf.Clamp(chestRotationX, clampAngleHorizontal.x, clampAngleHorizontal.y); ClampAngle(chestRotationX, clampAngleHorizontal.x, clampAngleHorizontal.y); }
            if (clampChestRotationVertical) { chestRotationY = Mathf.Clamp(chestRotationY, clampAngleVertical.x, clampAngleVertical.y); ClampAngle(chestRotationY, clampAngleVertical.x, clampAngleVertical.y); }

            //chest.rotation = Quaternion.Euler(chestRotationY, legsRotationX + chestRotationX, 0);
            chest.rotation = Quaternion.Euler(chestRotationY, legsBaseAngle + chestRotationX, 0);
        }
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

        Vector3 movementDirection = inputMovement.y * chest.forward + inputMovement.x * chest.right;

        if (movementDirection.magnitude >= 0.2f)
        {
            //Rotate smoothly to direction
            /*float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + chest.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);*/

            //Mecha movement
            //Vector3 directionForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            /*targetAngleChest = chest.eulerAngles.y - 180;
            turnDownBody = true;*/

            //legs.rotation = Quaternion.Euler(legs.rotation.eulerAngles.x, chest.eulerAngles.y - 180, legs.rotation.eulerAngles.z);
            rb.MovePosition(rb.position + movementDirection * speed * Time.deltaTime);
            mechaAnimationScript.WalkAnimation(true);
        }
        else mechaAnimationScript.WalkAnimation(false);

        if (turnDownBody)
        {
            float angle = Mathf.SmoothDampAngle(legs.eulerAngles.y, targetAngleChest, ref turnVelocity, turnTime);
            legs.rotation = Quaternion.Euler(legs.rotation.eulerAngles.x, angle, legs.rotation.eulerAngles.z);
        }
    }

    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
