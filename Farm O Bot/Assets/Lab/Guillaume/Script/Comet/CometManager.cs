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

    [HideInInspector] public GameObject clientPlayer;
    private uint targetCompteur = 0;

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
            RpcSpawnPieceOfComec();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RpcSpawnPieceOfComec()
    {
        actualComet = Instantiate(cometPrefab, transform.position, transform.rotation);
        ServerManager.Spawn(actualComet, null);

        SpawnPieceOfComet(actualComet);

    }


    [ObserversRpc]
    private void SpawnPieceOfComet(GameObject comet)
    {

        PieceOfComet actualScript = comet.GetComponent<PieceOfComet>();
        actualScript.target = FindTarget();
        actualScript._objectifFeedback = clientPlayer.GetComponent<ObjectifFeedback>();

        numOftarget += 1;
        if (numOftarget >= allTargets.Length - 1)
        {
            numOftarget = 0;
        }

        clientPlayer.GetComponent<ObjectifFeedback>().SpawnAlert(comet);
    }



    private Transform FindTarget()
    {
        return allTargets[numOftarget];
    }

    private void OnDestroy()
    {
        clientPlayer.GetComponent<ObjectifFeedback>().DestroyAlert();
    }
}
