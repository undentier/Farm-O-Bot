using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BisonUiNumber1 : MonoBehaviour
{
    // Start is called before the first frame update
    private HerdMovement bisonManager;
    public TextMeshProUGUI countText;

    void Start()
    {
        bisonManager = GameObject.FindGameObjectWithTag("BisonManager").GetComponent<HerdMovement>();
    }

    
    void Update()
    {
        countText.text = "x" + bisonManager.numberBisons.ToString();
    }
}
