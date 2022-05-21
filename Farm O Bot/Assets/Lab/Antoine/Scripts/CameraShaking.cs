using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaking : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineNoise.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineNoise.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1 - (shakeTimer / shakeTimerTotal)));
        }
    }
}
