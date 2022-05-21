using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadVibration : MonoBehaviour
{
    private Gamepad currentGamepad;

    private void Start()
    {
        if (Gamepad.all.Count > 0)
        {
            currentGamepad = Gamepad.all[0];
        }
    }

    private void Update()
    {
        if (Gamepad.all.Count > 0)
        {
            currentGamepad = Gamepad.all[0];
        }
        else
        {
            currentGamepad = null;
        }
    }

    public void VibrationWithTime(float vibrationTime, float leftMotorForce, float rightMotorForce)
    {
        if(currentGamepad != null) StartCoroutine(Vibration(vibrationTime, leftMotorForce, rightMotorForce));
    }

    public void StartVibration(float leftMotorForce, float rightMotorForce)
    {
        if (currentGamepad != null) currentGamepad.SetMotorSpeeds(leftMotorForce, rightMotorForce);
    }

    public void StopVibration()
    {
        if (currentGamepad != null) currentGamepad.ResetHaptics();
    }

    IEnumerator Vibration(float vibrationTime, float leftMotorForce, float rightMotorForce)
    {
        currentGamepad.SetMotorSpeeds(leftMotorForce, rightMotorForce);
        yield return new WaitForSeconds(vibrationTime);
        currentGamepad.ResetHaptics();
    }
}
