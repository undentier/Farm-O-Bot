using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class MechaControllerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float rotationSpeedLegs;
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
        legsRotationX += inputMovement.x * rotationSpeedLegs * Time.deltaTime;

        legs.rotation = Quaternion.Euler(legs.rotation.eulerAngles.x, legsRotationX + 180f, legs.rotation.eulerAngles.z);
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
        Vector3 movement = -chest.forward * -inputMovement.y;

        if (movement.magnitude > 0.3f) 
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
            mechaAnimationScript.WalkAnimation(true);
        }
        else
        {
            mechaAnimationScript.WalkAnimation(false);
        }
    }
}
