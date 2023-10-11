using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageMultiTrackingController2 : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager refToTrackedImageManager;

    [SerializeField]
    private MultiObjectImageScriptableObject scriptableObjectMOI;

    private void Awake()
    {
        scriptableObjectMOI.Init();
    }

    private void OnEnable()
    {
        refToTrackedImageManager.trackedImagesChanged += RefToTrackedImageManager_trackedImagesChanged;
    }

    private void OnDisable()
    {
        refToTrackedImageManager.trackedImagesChanged -= RefToTrackedImageManager_trackedImagesChanged;
    }

    private void RefToTrackedImageManager_trackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Gå igenom listan av de nykomliga trackade bilderna
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            // Variant 1
            /*
            GameObject ob = scriptableObjectMOI.GetObjectByImageName(trackedImage.referenceImage.name);

            if (ob != null)
            {
                ob.SetActive(true);
                ob.transform.position = trackedImage.transform.position;
                ob.transform.rotation = trackedImage.transform.rotation;
            }
            */

            // Variant 2 (gör detta istället för variant 1)
            string imageName = trackedImage.referenceImage.name;

            scriptableObjectMOI.ChangeActive(imageName, true);
            scriptableObjectMOI.UpdatePositionRotation(imageName, trackedImage.transform);

        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            string imageName = trackedImage.referenceImage.name;

            GameObject ob = scriptableObjectMOI.GetObjectByImageName(imageName);

            if (ob != null)
            {
                scriptableObjectMOI.UpdatePositionRotation(imageName, trackedImage.transform);

                if (trackedImage.trackingState == TrackingState.Limited)
                {
                
                    scriptableObjectMOI.ChangeActive(imageName, false);
                
                } else if (trackedImage.trackingState == TrackingState.Tracking) 
                {

                    scriptableObjectMOI.ChangeActive(imageName, true);
                
                }
            }
        }

        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        {
            string imageName = trackedImage.referenceImage.name;

            scriptableObjectMOI.ChangeActive(imageName, false);
        }
    }
}
