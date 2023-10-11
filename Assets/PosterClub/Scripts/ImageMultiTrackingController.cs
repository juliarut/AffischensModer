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
        // Här lägger vi till vilken bilds namn ska associeras med vilket gameobject
        // så det som läggs till här finns sedan tillgängligt senare i körningen
        // resten ignoreras. dvs om ni har en annan bild som inte har namnet inlagt här
        // så händer ingenting
        foreach(var arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name; // kopiera över namnet från objektet till namnet på kopian
            newARObject.SetActive(false); // dölj kopian
            arObjects.Add(arObject.name, newARObject); // För över allt nu (dvs kopian) till dictionary
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
        // Gå igenom listan av de nykomliga trackade bilderna
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            // Kolla nu om den trackade bildens namn överenstämmer med namnet på nyckeln i  
            // Dictionarylistan arObjects, dvs om den finns med där eller ej
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
