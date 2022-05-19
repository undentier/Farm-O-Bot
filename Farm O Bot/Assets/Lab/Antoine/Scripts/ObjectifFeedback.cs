using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class ObjectifFeedback : NetworkBehaviour
{
    public Transform canvas;
    public GameObject cometAlert;
    public Vector3 alertOffset;

    public List<Image> alertImages;
    public List<Transform> comets;

    private Camera mechaCam;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            CometManager.instance.clientPlayer = gameObject;
            mechaCam = GetComponent<MechaCamera>().mainCamera.GetComponent<Camera>();
        }
    }

    public void SpawnAlert(GameObject currentComet)
    {
        if (IsOwner)
        {
            GameObject currentAlert = Instantiate(cometAlert, canvas.position, Quaternion.identity, canvas);
            alertImages.Add(currentAlert.GetComponent<Image>());
            comets.Add(currentComet.transform);
        }
    }

    public void DestroyAlert()
    {
        if (IsOwner)
        {
            Destroy(alertImages[0].gameObject);
            alertImages.RemoveAt(0);
            comets.RemoveAt(0);
        }
    }

    private void Update()
    {
        if (comets.Count > 0 && IsOwner)
        {
            for (int i = 0; i < alertImages.Count; i++)
            {
                //Clamp image in screen
                float minX = alertImages[i].GetPixelAdjustedRect().width / 2;
                float maxX = Screen.width - minX;

                float minY = alertImages[i].GetPixelAdjustedRect().height / 2;
                float maxY = Screen.height - minY;

                Vector2 alertPos = mechaCam.WorldToScreenPoint(comets[i].position + alertOffset);

                if (Vector3.Dot((comets[i].position - mechaCam.transform.position), mechaCam.transform.forward) < 0)
                {
                    //Comet is behind the player camera
                    if(alertPos.x < Screen.width / 2)
                    {
                        alertPos.x = maxX;
                    }
                    else
                    {
                        alertPos.x = minX;
                    }
                }

                alertPos.x = Mathf.Clamp(alertPos.x, minX, maxX);
                alertPos.y = Mathf.Clamp(alertPos.y, minY, maxY);

                alertImages[i].transform.position = alertPos;
            }
        }
    }
}
