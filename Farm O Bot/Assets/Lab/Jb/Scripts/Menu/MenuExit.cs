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
    private NewMechaControllerMovement _mechaControllerMovement;
    [SerializeField]
    private MechaAiming _mechaAiming;
    [SerializeField]
    private PlayerFire _playerFire;
    [SerializeField]
    private BuildSystem _buildSystem;
    [SerializeField]
    private MechaAnimation _mechaAnimation;

    private bool openMenu = false;

    private void Start()
    {
        uiExitButton.SetActive(false);
        _playerInput.actions["Menu"].started += OpenCloseMenu;
    }

    private void OpenCloseMenu(InputAction.CallbackContext context)
    {
        if (context.started && _mechaControllerMovement.IsOwner) 
        {
            openMenu = !openMenu;
            if (openMenu)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                DisablePlayerScript(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                DisablePlayerScript(true);
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

    private void DisablePlayerScript(bool enable)
    {
        _mechaControllerMovement.inPause = !enable;
        _mechaControllerMovement.decelerationTimer = _mechaControllerMovement.decelerationTime;
        _mechaAiming.enabled = enable;
        _playerFire.enabled = enable;
        _buildSystem.enabled = enable;
        _mechaAnimation.WalkAnimation(false);
        _mechaAnimation.WalkAnimation(false);
    }
}
