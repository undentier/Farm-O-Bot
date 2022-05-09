using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometManager : MonoBehaviour
{
    #region

    public static CometManager instance;

    [Header("Target possible")]
    public Transform[] allTargets;

    [Header ("Unity setup")]
    public GameObject cometPrefab;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPieceOfComet();
        }
    }


    private void SpawnPieceOfComet()
    {
        GameObject actualCometPiece = Instantiate(cometPrefab, transform.position, transform.rotation);
        PieceOfComet actualScript = actualCometPiece.GetComponent<PieceOfComet>();

        actualScript.target = FindTarget();
    }

    private Transform FindTarget()
    {
        int random = Random.Range(0, allTargets.Length - 1);

        return allTargets[random];
    }
}
