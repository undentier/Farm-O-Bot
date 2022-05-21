using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Managing;

public class MenuConnection : MonoBehaviour
{
    [SerializeField]
    private NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressedHost()
    {
        if (networkManager == null)
            return;

        networkManager.ServerManager.StopConnection(true);
        networkManager.ServerManager.StartConnection();

    }

    public void OnPressedJoin()
    {
        if (networkManager == null)
            return;

        networkManager.ClientManager.StopConnection();
        networkManager.ClientManager.StartConnection();


    }

    private void StartConnection()
    {
        
    }
}
