using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using System;

public class PlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;

    public GameObject objectToPlace;

    public LoadAssetBundle loadAssetBundle;

    private ARSessionOrigin arOrigin;

    private ARRaycastManager raycastManager;

    private Pose placementPose;

    private bool isValidPose;

    bool isPlayerCame = false;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();

        raycastManager = arOrigin.GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if(isValidPose && Input.touchCount > 0 && Input.GetTouch(0).phase==TouchPhase.Began && !isPlayerCame)
        {
            isPlayerCame = true;
            loadAssetBundle.LoadPlayer(placementPose.position, placementPose.rotation);
            //PlaceTheObject();
        }
    }

    private void PlaceTheObject()
    {
         Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if(isValidPose)
        {
            placementIndicator.SetActive(true);

            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        var hits = new List<ARRaycastHit>();

        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        isValidPose = hits.Count > 0;

        if(isValidPose)
        {
            placementPose = hits[0].pose;

            var cameraForForward = Camera.current.transform.forward;

            var cameraBearing = new Vector3(cameraForForward.x, 0, cameraForForward.z).normalized;

            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
