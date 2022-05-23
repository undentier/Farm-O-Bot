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
    [HideInInspector] public float decelerationTimer;
    private bool isMoving = false;
    private Vector3 movementDirection;
    private Vector3 gravityMovement;
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

    [Header("Feedback")]
    public float camShakeIntensityWalk;
    public float camShakeDurationWalk;
    public float camShakeFrequenceWalking;
    public float walkVibrationDuration;
    public float walkLeftVibrationIntensity;
    public float walkRightVibrationIntensity;

    [Space]
    public float camShakeIntensityRun;
    public float camShakeDurationRun;
    public float camShakeFrequenceRunning;
    private float camShakeFrequenceTimer;
    public float runVibrationDuration;
    public float runLeftVibrationIntensity;
    public float runRightVibrationIntensity;

    //References
    private Rigidbody rb;
    private DefaultInputActions playerActions;
    private MechaAnimation mechaAnimationScript;
    private EnergySystem _energySystem;
    private PlayerInput _playerInput;
    [HideInInspector] public CharacterController _characterController;
    private CameraShaking _cameraShaking;
    private GamepadVibration _gamepadVibration;

    [HideInInspector] public bool inPause = false;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraShaking = GetComponent<MechaCamera>().cinecam.GetComponent<CameraShaking>();
        _energySystem = GetComponent<EnergySystem>();
        _gamepadVibration = GetComponent<GamepadVibration>();
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
        if (!inPause)
        {
            if (IsOwner)
            {
                ReadInput();

                UseGravity();
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

    private void UseGravity()
    {
        if (_characterController.isGrounded)
        {
            gravityMovement.y = -0.05f;
        }
        else
        {
            gravityMovement.y = -9.8f;
        }
    }

    private void MoveMecha()
    {
        movementDirection = inputMovement.y * -legs.forward + inputMovement.x * legs.up;

        if (movementDirection.magnitude >= 0.2f)
        {
            ResetAngleChestAndLegs();
            if (isRunning) /*rb.MovePosition(rb.position + movementDirection.normalized * boostedSpeed * Time.deltaTime)*/ _characterController.Move((movementDirection.normalized + gravityMovement) * boostedSpeed * Time.deltaTime);
            else /*rb.MovePosition(rb.position + movementDirection.normalized * normalSpeed * Time.deltaTime)*/ _characterController.Move((movementDirection.normalized + gravityMovement) * normalSpeed * Time.deltaTime);
            decelerationTimer = 0;
            isMoving = true;
            lastMovementDirection = movementDirection;
            lastMovementDirection.Normalize();
            turnTime = turnTimeInMovement;
            TurnLegsDependingChest();
            ApplyCamShake();
        }
        else
        {
            if (decelerationTimer <= decelerationTime)
            {
                decelerationTimer += Time.deltaTime;
                //rb.MovePosition(rb.position + lastMovementDirection * decelerationSpeed * Time.deltaTime);
                _characterController.Move((lastMovementDirection + gravityMovement) * decelerationSpeed * Time.deltaTime);
                isRunning = false;
            }
            else
            {
                if (isMoving)
                {
                    isMoving = false;
                    _cameraShaking.ShakeCamera(camShakeIntensityWalk, camShakeDurationWalk);
                    if (!_energySystem.mechaIsFire) _gamepadVibration.VibrationWithTime(walkVibrationDuration, walkLeftVibrationIntensity, walkRightVibrationIntensity);
                    camShakeFrequenceTimer = 0;
                }
            }
        }
    }

    private void ApplyCamShake()
    {
        if (!isRunning)
        {
            if (camShakeFrequenceTimer < camShakeFrequenceWalking)
            {
                camShakeFrequenceTimer += Time.deltaTime;
            }
            else
            {
                _cameraShaking.ShakeCamera(camShakeIntensityWalk, camShakeDurationWalk);
                if(!_energySystem.mechaIsFire) _gamepadVibration.VibrationWithTime(walkVibrationDuration, walkLeftVibrationIntensity, walkRightVibrationIntensity);
                camShakeFrequenceTimer = 0;
            }
        }
        else
        {
            if (camShakeFrequenceTimer < camShakeFrequenceRunning)
            {
                camShakeFrequenceTimer += Time.deltaTime;
            }
            else
            {
                _cameraShaking.ShakeCamera(camShakeIntensityRun, camShakeDurationRun);
                if (!_energySystem.mechaIsFire) _gamepadVibration.VibrationWithTime(runVibrationDuration, runLeftVibrationIntensity, runRightVibrationIntensity);
                camShakeFrequenceTimer = 0;
            }
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

    public void ChangeSensitivity(float sens)
    {
        if (Gamepad.all.Count > 0)
        {
            rotationSpeedChest = 100 * sens * 3;
        }

        mouseSensibility = sens * 2;
    }
}
