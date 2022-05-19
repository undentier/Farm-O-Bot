using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet;

public class CometManager : NetworkBehaviour
{
    #region

    public static CometManager instance;

    [Header("Target possible")]
    public Transform[] allTargets;

    [Header ("Unity setup")]
    public GameObject cometPrefab;

    public PlayerInput playerInputScript;

    private int numOftarget;
    private GameObject actualComet;

    #endregion

    private void Awake()
    {
        #region Setup instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Care there is more than one CometManager scrpit in the scene");
        }
        #endregion
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        playerInputScript.actions["SpawnComet"].started += Spawn;
        playerInputScript.actions["SpawnComet"].canceled += Spawn;
    }

    public void Spawn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SpawnPieceOfComet();
        }
    }


    [ObserversRpc]
    private void SpawnPieceOfComet()
    {
        actualComet = Instantiate(cometPrefab, transform.position, transform.rotation);

        PieceOfComet actualScript = actualComet.GetComponent<PieceOfComet>();
        actualScript.target = FindTarget();
        actualScript._objectifFeedback = GameManager.instance.clientPlayer.GetComponent<ObjectifFeedback>();

        numOftarget += 1;
        if (numOftarget >= allTargets.Length - 1)
        {
            numOftarget = 0;
        }

        GameManager.instance.clientPlayer.GetComponent<ObjectifFeedback>().SpawnAlert(actualComet);
    }



    private Transform FindTarget()
    {
        return allTargets[numOftarget];
    }
}
