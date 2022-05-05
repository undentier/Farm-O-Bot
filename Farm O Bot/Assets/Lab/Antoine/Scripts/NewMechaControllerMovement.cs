using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMechaControllerMovement : MonoBehaviour
{
    [Header("Movement")]
    [Range(0, 50)]
    public float speed;
    private Vector3 movementDirection;
    [Range(0, 5)]
    public float turnTimeStatic;
    [Range(0, 5)]
    public float turnTimeInMovement;
    private float turnTime;
    private float turnVelocity;

    private float targetAngleChest = 0;
    private float legsBaseAngle;
    public Transform legs;

    [Header("Aim")]
    [Range(0, 300)]
    public float rotationSpeedChest;
    public bool clampChestRotationHorizontal = false;
    public Vector2 clampAngleHorizontal;
    public bool clampChestRotationVertical = false;
    public Vector2 clampAngleVertical;
    private float chestRotationX;
    private float chestRotationY;
    public Transform chest;

    //Input
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

        ResetAngleChestAndLegs();
    }

    private void Update()
    {
        ReadInput();

        MoveMecha();
        RotateMechaChest();
        if (clampChestRotationHorizontal && movementDirection.magnitude <= 0.2f) RotateMechaLegs();
    }

    private void ReadInput()
    {
        inputMovement = playerActions.Player.Move.ReadValue<Vector2>();
        inputLook = playerActions.Player.Look.ReadValue<Vector2>();
    }

    private void MoveMecha()
    {
        movementDirection = inputMovement.y * chest.forward + inputMovement.x * chest.right;

        if (movementDirection.magnitude >= 0.2f)
        {
            ResetAngleChestAndLegs();

            turnTime = turnTimeInMovement; ;
            TurnLegsDependingChest();

            rb.MovePosition(rb.position + movementDirection * speed * Time.deltaTime);
            mechaAnimationScript.WalkAnimation(true);
        }
        else mechaAnimationScript.WalkAnimation(false);

        
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
        Vector3 lookDirection = Vector2.ClampMagnitude(new Vector2(inputLook.x, inputLook.y), 1);

        if (lookDirection.magnitude >= 0.2f)
        {
            chestRotationX += lookDirection.x * rotationSpeedChest * Time.deltaTime;
            chestRotationY += -lookDirection.y * rotationSpeedChest * Time.deltaTime;

            if (clampChestRotationHorizontal) { chestRotationX = Mathf.Clamp(chestRotationX, clampAngleHorizontal.x, clampAngleHorizontal.y); ClampAngle(chestRotationX, clampAngleHorizontal.x, clampAngleHorizontal.y); }
            if (clampChestRotationVertical) { chestRotationY = Mathf.Clamp(chestRotationY, clampAngleVertical.x, clampAngleVertical.y); ClampAngle(chestRotationY, clampAngleVertical.x, clampAngleVertical.y); }

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

    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
