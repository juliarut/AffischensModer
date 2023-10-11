using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageAnchorController : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    private GameObject objectPrefabToPlace;

    private GameObject spawnedObject;

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += TrackedImageManager_trackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= TrackedImageManager_trackedImagesChanged;
    }

    private void TrackedImageManager_trackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage newImage in eventArgs.added)
        {
            // H�r g�r vi n�got med trackade bilderna n�r de f�rsta g�ngen trackades
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(objectPrefabToPlace, newImage.transform.position, newImage.transform.rotation);

                if (spawnedObject.GetComponent<ARAnchor>() == null)
                    spawnedObject.AddComponent<ARAnchor>();
            }
        }

        foreach (ARTrackedImage updateImage in eventArgs.updated)
        {
            //
        }
        
        foreach (ARTrackedImage removeImage in eventArgs.removed)
        {
            // 
        }

    }


}
