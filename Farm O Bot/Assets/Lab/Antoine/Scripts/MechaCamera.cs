using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FishNet.Object;

public class MechaCamera : NetworkBehaviour
{
    [Header("TPS cam")]
    public CinemachineVirtualCamera cinecam;
    public GameObject mainCamera;
    public Transform mechCameraRoot;

    [Header("Mini map cam")]
    public GameObject minimapCamera;
    public Color myMecha, otherMecha;
    public SpriteRenderer iconPlayer;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            mainCamera.SetActive(true);
            cinecam.gameObject.SetActive(true);
            cinecam.Follow = mechCameraRoot;
            minimapCamera.SetActive(true);

            if (GameManager.instance.playerTransformList.Count > 1)
            {
                iconPlayer.color = otherMecha;
            }
            else iconPlayer.color = myMecha;
        }
        else
        {
            cinecam.gameObject.SetActive(false);
            mainCamera.SetActive(false);
            minimapCamera.SetActive(false);
        }
    }
}
