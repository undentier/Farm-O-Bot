using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Managing;
using TMPro;
using FishNet.Transporting.Tugboat;
using UnityEngine.SceneManagement;
using FishNet;

public class MenuConnection : MonoBehaviour
{
    [SerializeField]
    private NetworkManager networkManager;
    [SerializeField]
    private TMP_InputField ipInput;
    [SerializeField]
    private Tugboat tugboat;
    [SerializeField]
    private string sceneToLoad = "CleanSceneBoids";
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

        
        SceneManager.LoadScene(sceneToLoad);

        InstanceFinder.ServerManager.StopConnection(true);
        InstanceFinder.ServerManager.StartConnection();

        tugboat.SetClientAddress("localhost");
        InstanceFinder.ClientManager.StopConnection();
        InstanceFinder.ClientManager.StartConnection();

    }

    public void OnPressedJoin()
    {
        if (networkManager == null)
            return;

        SceneManager.LoadScene(sceneToLoad);

        tugboat.SetClientAddress(ipInput.text);
        InstanceFinder.ClientManager.StopConnection();
        InstanceFinder.ClientManager.StartConnection();

    }



}
