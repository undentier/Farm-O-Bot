using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    public float maxEnergy;
    public float regainEnergyRate;
    private float currentEnergy;

    public Image energyImage;

    private void Start()
    {
        energyImage.fillAmount = 0;
    }

    public void RemoveEnergy(float valueRemove)
    {
        currentEnergy -= valueRemove;
        energyImage.fillAmount = maxEnergy / currentEnergy;
    }

    private void Update()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += Time.deltaTime * regainEnergyRate;
        }
    }
}
