using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceShowHidePlane : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToInstantiate;

    private GameObject spawnedObject = null;

    [SerializeField]
    private ARRaycastManager raycastManager;

    private static List<ARRaycastHit> hitList = new List<ARRaycastHit>();

    [SerializeField]
    private ARPlaneManager planeManager;

    bool TryGetTouchPosition(out Vector2 touchPos)
    {
        if (Input.touchCount > 0)
        {
            touchPos = Input.GetTouch(0).position;
            return true;
        }

        touchPos = default;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (raycastManager.Raycast(touchPosition, hitList, TrackableType.Planes))
        {
            Pose hitPose = hitList[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(prefabToInstantiate, hitPose.position, hitPose.rotation);

                TogglePlaneDetection();
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
                spawnedObject.transform.rotation = hitPose.rotation;
            }
        }


    }

    // Funktion för att växla mellan att visa och dölja alla plan som har trackats
    public void TogglePlaneVisibilityStillTracking()
    {
        // Kontrollera att planeManager är inställd och inte null
        if (planeManager != null)
        {
            // Växla planeManager mellan aktiverad och inaktiverad
            // Tack vare vi gör denna rad kommer vi även "förstöra" tidigare trackingen för dessa plan
            planeManager.enabled = !planeManager.enabled;

            // För varje plan i planeManager-trackables, aktivera eller inaktivera dess GameObject
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(planeManager.enabled);
            }
        }
    }

    /// <summary>
    /// Detta döljer/visar UTAN att förstöra dem
    /// </summary>
    public void TogglePlaneVisibility()
    {
        if (planeManager != null)
        {
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(!plane.gameObject.activeSelf);
            }
        }
    }


    // Funktion för att sluta tracka plan
    public void StopPlaneTracking()
    {
        // Kontrollera att planeManager är inställd och inte null
        if (planeManager != null)
        {
            // Inaktivera planeManager för att sluta tracka plan
            planeManager.enabled = false;
        }
    }

    // Funktion för att återuppta tracking av plan
    public void ResumePlaneTracking()
    {
        // Kontrollera att planeManager är inställd och inte null
        if (planeManager != null)
        {
            // Aktivera planeManager för att återuppta tracking av plan
            planeManager.enabled = true;
        }
    }





    bool isPlaneToggled;

    public void TogglePlaneDetection()
    {
        /*
        if (isPlaneToggled)
        {
            isPlaneToggled = false;
        } else
        {
            isPlaneToggled = true;
        }
        */

        isPlaneToggled = !isPlaneToggled;

        planeManager.SetTrackablesActive(isPlaneToggled);
        // planeManager.planePrefab.SetActive(isPlaneToggled);

        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(isPlaneToggled);
        }
    }
}
