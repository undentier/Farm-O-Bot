using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using FishNet.Object;

public class EnergySystem : NetworkBehaviour
{
    public float maxEnergy;
    [HideInInspector] public float currentEnergy = 0;
    [HideInInspector] public bool energyFulled = false;
    [HideInInspector] public bool isCooldown = false;
    [HideInInspector] public bool mechaIsFire = false;

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
        CheckEnergy();

        if (currentEnergy > 0 && IsOwner && (!mechaIsFire) || isCooldown)
        {
            currentEnergy -= Time.deltaTime * regainEnergyRate;
            energyImage.fillAmount = currentEnergy / maxEnergy;
        }

        if (currentEnergy >= maxEnergy && IsOwner)
        {
            currentEnergy = maxEnergy - 0.1f;
        }
    }

    private void CheckEnergy()
    {
        if (IsOwner && (currentEnergy < maxEnergy))
        {
            energyFulled = false;
        }
        if (IsOwner && (currentEnergy >= maxEnergy))
        {
            energyFulled = true;
        }
    }
}
