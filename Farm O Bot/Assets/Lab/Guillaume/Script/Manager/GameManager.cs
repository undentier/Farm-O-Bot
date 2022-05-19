using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public GameObject clientPlayer;
    public List<Transform> playerTransformList = new List<Transform>();
    public Transform bisonTransform;

    private void Awake()
    {
        #region Setup Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There is multiple GameManager in the scene");
        }
        #endregion
    }
}
