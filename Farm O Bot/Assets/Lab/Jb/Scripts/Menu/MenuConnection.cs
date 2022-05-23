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
    private NetworkManager networkManager;
    [SerializeField]
    private TMP_InputField ipInput;
    private Tugboat tugboat;
    [SerializeField]
    private string sceneToLoad = "CleanSceneBoids";
    // Start is called before the first frame update

    public void Start()
    {
        networkManager = InstanceFinder.ServerManager.NetworkManager;
        tugboat = networkManager.GetComponent<Tugboat>();
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


    public void Quit()
    {
        Application.Quit();
    }
}
