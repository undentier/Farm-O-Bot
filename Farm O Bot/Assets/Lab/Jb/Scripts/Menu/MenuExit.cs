using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuExit : MonoBehaviour
{
    [SerializeField]
    private GameObject uiExitButton;
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private NewMechaControllerMovement NewMechaControllerMovement;

    private bool openMenu = false;

    private void Start()
    {
        uiExitButton.SetActive(false);
        _playerInput.actions["Menu"].started += OpenCloseMenu;
    }

    private void OpenCloseMenu(InputAction.CallbackContext context)
    {
        if (context.started && NewMechaControllerMovement.IsOwner) 
        {
            openMenu = !openMenu;
            if (openMenu)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            uiExitButton.SetActive(openMenu);
        }
    }

    public void OnPressedExit()
    {
        InstanceFinder.ServerManager.StopConnection(true);
        InstanceFinder.ClientManager.StopConnection();
        SceneManager.LoadScene("MenuScene");
    }

    public void OnDestroy()
    {
        _playerInput.actions["Menu"].started -= OpenCloseMenu;
    }
}
