using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using UnityEngine.SceneManagement;

public class MenuExit : MonoBehaviour
{
    [SerializeField]
    private GameObject uiExitButton;
    private void Start()
    {
        //uiExitButton.SetActive(false);
    }
    public void OnPressedExit()
    {
        InstanceFinder.ServerManager.StopConnection(true);
        SceneManager.LoadScene("MenuScene");
    }
}
