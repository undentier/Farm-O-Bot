using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EnergySystem : MonoBehaviour
{
    public float maxEnergy;
    [HideInInspector] public float currentEnergy = 0;

    public float regainEnergyRate;

    public Image energyImage;

    private InputActionReference inputLeft;
    private InputActionReference inputLRight;

    private void Start()
    {
        energyImage.fillAmount = 0;

        inputLeft = GetComponent<PlayerFire>().leftFireInput;
        inputLRight = GetComponent<PlayerFire>().rightFireInput;
    }

    public void AddEnergy(float valueRemove)
    {
        if (currentEnergy <= maxEnergy)
        {
            currentEnergy += valueRemove;
            energyImage.fillAmount = currentEnergy / maxEnergy;
        }
    }

    private void Update()
    {
        if (currentEnergy > 0 && currentEnergy < maxEnergy)
        {
            currentEnergy -= Time.deltaTime * regainEnergyRate;
            energyImage.fillAmount = currentEnergy / maxEnergy;
        }

        if (inputLeft.action.phase == InputActionPhase.Waiting && inputLRight.action.phase == InputActionPhase.Waiting && currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy - 0.1f;
        }
    }
}
