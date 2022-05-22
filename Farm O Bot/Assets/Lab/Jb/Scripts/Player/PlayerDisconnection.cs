using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;
using UnityEngine.SceneManagement;
public class PlayerDisconnection : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InstanceFinder.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
    }

    private void ClientManager_OnClientConnectionState(FishNet.Transporting.ClientConnectionStateArgs obj)
    {
        switch (obj.ConnectionState)
        {
            case FishNet.Transporting.LocalConnectionStates.Stopped:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
                Cursor.lockState = CursorLockMode.None;
                break;
            case FishNet.Transporting.LocalConnectionStates.Starting:
                break;
            case FishNet.Transporting.LocalConnectionStates.Started:
                break;
            case FishNet.Transporting.LocalConnectionStates.Stopping:
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
