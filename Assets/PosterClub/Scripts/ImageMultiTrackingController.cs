using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageMultiTrackingController : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager refToTrackedImageManager;

    [SerializeField]
    private List<GameObject> arObjectsToPlace = new List<GameObject>();

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        // H�r l�gger vi till vilken bilds namn ska associeras med vilket gameobject
        // s� det som l�ggs till h�r finns sedan tillg�ngligt senare i k�rningen
        // resten ignoreras. dvs om ni har en annan bild som inte har namnet inlagt h�r
        // s� h�nder ingenting
        foreach(var arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name; // kopiera �ver namnet fr�n objektet till namnet p� kopian
            newARObject.SetActive(false); // d�lj kopian
            arObjects.Add(arObject.name, newARObject); // F�r �ver allt nu (dvs kopian) till dictionary
        }
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
        // G� igenom listan av de nykomliga trackade bilderna
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            // Kolla nu om den trackade bildens namn �verenst�mmer med namnet p� nyckeln i  
            // Dictionarylistan arObjects, dvs om den finns med d�r eller ej
            // om den finns med 
            if (arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                arObjects[trackedImage.referenceImage.name].SetActive(true);
                arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                arObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
            }
        }

        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                arObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
               
                if (trackedImage.trackingState == TrackingState.Limited)
                {
                    arObjects[trackedImage.referenceImage.name].SetActive(false);
                } else if (trackedImage.trackingState == TrackingState.Tracking) {
                    arObjects[trackedImage.referenceImage.name].SetActive(true);
                } 
            }
        }

        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                arObjects[trackedImage.referenceImage.name].SetActive(false);
            }
        }
    }
}
