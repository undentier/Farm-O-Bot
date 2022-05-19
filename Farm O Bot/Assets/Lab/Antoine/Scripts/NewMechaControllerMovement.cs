using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
public class NewMechaControllerMovement : NetworkBehaviour
{
    [Header("Mouse sensibility")]
    [Range(0, 1)] public float mouseSensibility;

    [Header("Movement")]
    /*[Range(0, 50)]*/
    public float normalSpeed;
    /*[Range(0, 50)]*/
    public float boostedSpeed;
    [Range(0, 50)]
    public float decelerationSpeed;
    public float decelerationTime;
    private float decelerationTimer;
    private bool isMoving = false;
    private Vector3 movementDirection;
    private Vector3 lastMovementDirection;
    private bool isRunning = false;

    public float gravity = 0;

    [Header("Legs")]
    public float turnTimeStatic;
    public float turnTimeInMovement;
    private float turnTime;
    private float turnVelocity;

    private float targetAngleChest = 0;
    private float legsBaseAngle;
    public Transform legs;

    [Header("Chest")]
    [Range(0, 300)]
    public float rotationSpeedChest;
    public bool clampChestRotationHorizontal = false;
    public Vector2 clampAngleHorizontal;
    public bool clampChestRotationVertical = false;
    public Vector2 clampAngleVertical;
    private float chestRotationX;
    private float chestRotationY;
    public Transform chest;
    public Transform lookAtReticule;
    //public Transform mechCameraRoot;
    

    //Input
    private Vector2 inputMovement;
    private Vector2 inputLook;

    //References
    private Rigidbody rb;
    private DefaultInputActions playerActions;
    private MechaAnimation mechaAnimationScript;
    private PlayerInput _playerInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mechaAnimationScript = GetComponent<MechaAnimation>();
        playerActions = new DefaultInputActions();
        playerActions.Player.Enable();
        ResetAngleChestAndLegs();

        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Run"].started += ReadRunInput;
        _playerInput.actions["Run"].canceled += ReadRunInput;
    }

    private void Update()
    {
        if (IsOwner)
        {
            ReadInput();

            MoveMecha();
            RotateMechaChest();
            if (clampChestRotationHorizontal && movementDirection.magnitude <= 0.2f) RotateMechaLegs();

            AnimateMecha();
        }

        if (!isMoving && IsOwner)
        {
            isRunning = false;
        }
    }

    private void ReadInput()
    {
        inputMovement = playerActions.Player.Move.ReadValue<Vector2>();
        inputLook = playerActions.Player.Look.ReadValue<Vector2>();
    }

    private void ReadRunInput(InputAction.CallbackContext context)
    {
        if (context.started && IsOwner)
        {
            isRunning = true;
        }
    }

    private void MoveMecha()
    {
        movementDirection = inputMovement.y * -legs.forward + inputMovement.x * legs.up;

        if (movementDirection.magnitude >= 0.2f)
        {
            ResetAngleChestAndLegs();
            if (isRunning) rb.MovePosition(rb.position + movementDirection.normalized * boostedSpeed * Time.deltaTime);
            else rb.MovePosition(rb.position + movementDirection.normalized * normalSpeed * Time.deltaTime);
            decelerationTimer = 0;
            isMoving = true;
            lastMovementDirection = movementDirection;
            lastMovementDirection.Normalize();
            turnTime = turnTimeInMovement;
            TurnLegsDependingChest();
        }
        else
        {
            if (decelerationTimer <= decelerationTime)
            {
                decelerationTimer += Time.deltaTime;
                rb.MovePosition(rb.position + lastMovementDirection * decelerationSpeed * Time.deltaTime);
            }
            else isMoving = false;
        }
    }

    private void RotateMechaLegs()
    {
        if ((chestRotationX >= clampAngleHorizontal.y || chestRotationX <= clampAngleHorizontal.x))
        {
            turnTime = turnTimeStatic;
            ResetAngleChestAndLegs();
        }

        TurnLegsDependingChest();
    }

    private void RotateMechaChest()
    {
        //Vector3 lookDirection = Vector2.ClampMagnitude(new Vector2(inputLook.x, inputLook.y), 1);
        Vector3 lookDirection = new Vector2(inputLook.x, inputLook.y);

        if (lookDirection.magnitude >= 0.2f)
        {
            //A controller is plugged
            if (Gamepad.all.Count > 0)
            {
                chestRotationX += lookDirection.x * rotationSpeedChest * Time.deltaTime;
                chestRotationY += -lookDirection.y * rotationSpeedChest * Time.deltaTime;
            }
            //Mouse is use
            else
            {
                chestRotationX += lookDirection.x * rotationSpeedChest * (mouseSensibility * 0.1f) * Time.deltaTime;
                chestRotationY += -lookDirection.y * rotationSpeedChest * (mouseSensibility * 0.1f) * Time.deltaTime;
            }

            if (clampChestRotationHorizontal) { chestRotationX = Mathf.Clamp(chestRotationX, clampAngleHorizontal.x, clampAngleHorizontal.y); ClampAngle(chestRotationX, clampAngleHorizontal.x, clampAngleHorizontal.y); }
            if (clampChestRotationVertical) { chestRotationY = Mathf.Clamp(chestRotationY, clampAngleVertical.x, clampAngleVertical.y); ClampAngle(chestRotationY, clampAngleVertical.x, clampAngleVertical.y); }

            //chest.rotation = Quaternion.Euler(lookAtReticule.eulerAngles.x, legsBaseAngle + chestRotationX, 0);
            chest.rotation = Quaternion.Euler(chestRotationY, legsBaseAngle + chestRotationX, 0);
        }
        else
        {
            chest.rotation = Quaternion.Euler(chestRotationY, legsBaseAngle + chestRotationX, 0);
        }
    }

    private void ResetAngleChestAndLegs()
    {
        targetAngleChest = chest.eulerAngles.y - 180;
        chestRotationX = 0;
        legsBaseAngle = (targetAngleChest - 180);
    }

    private void TurnLegsDependingChest()
    {
        float angle = Mathf.SmoothDampAngle(legs.eulerAngles.y, targetAngleChest, ref turnVelocity, turnTime);
        legs.rotation = Quaternion.Euler(legs.rotation.eulerAngles.x, angle, legs.rotation.eulerAngles.z);
    }

    private void AnimateMecha()
    {
        if (isMoving)
        {
            if (isRunning)
            {
                mechaAnimationScript.WalkAnimation(false);
                mechaAnimationScript.RunAnimation(true);
            }
            else
            {
                mechaAnimationScript.WalkAnimation(true);
                mechaAnimationScript.RunAnimation(false);
            }
        }
        else
        {
            mechaAnimationScript.WalkAnimation(false);
            mechaAnimationScript.RunAnimation(false);
        }
    }

    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
