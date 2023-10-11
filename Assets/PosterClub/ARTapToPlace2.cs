using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlace2 : MonoBehaviour
{
    [SerializeField]
    private GameObject refToPrefab;

    [SerializeField]
    private ARRaycastManager raycastManager;

    private static List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    private List<GameObject> _listOfObjects = new List<GameObject>();

    private GameObject spawnedObject;

    private bool hasBeenCreated = false;

    /// <summary>
    /// Funktion som returnerar sant/falskt beroende på
    /// om vi tryckte på mobilskärmen eller ej.
    /// </summary>
    /// <param name="touchPos"></param>
    /// <returns></returns>
    bool TryGetTouchPosition(out Vector2 touchPos)
    {
        if (Input.touchCount > 0)
        {
            // Läser av vart jag tryckt på mobilskärmen
            // den returnerar tillbaks positionen vart jag tryckte
            // någonstans där och kopierar över värdet till touchPos
            touchPos = Input.GetTouch(0).position;
            
            return true;
        }

        touchPos = default; // Lägg märke till nyckelordet "default"

        return false;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPos))
        {
            return;
        }

        if (raycastManager.Raycast(touchPos, hitResults, TrackableType.Planes ))
        {
            // Få ut information 
            Pose hitPose = hitResults[0].pose;

            if (spawnedObject == null && hasBeenCreated == false)
            {
                spawnedObject = Instantiate(refToPrefab, hitPose.position, hitPose.rotation);
                hasBeenCreated = true;
            }
        }
    }

    public void CreateObject()
    {
        hasBeenCreated = false;
        Destroy(spawnedObject);
        spawnedObject = null;
    }
}
