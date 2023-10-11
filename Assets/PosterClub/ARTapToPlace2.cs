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
    /// Funktion som returnerar sant/falskt beroende p�
    /// om vi tryckte p� mobilsk�rmen eller ej.
    /// </summary>
    /// <param name="touchPos"></param>
    /// <returns></returns>
    bool TryGetTouchPosition(out Vector2 touchPos)
    {
        if (Input.touchCount > 0)
        {
            // L�ser av vart jag tryckt p� mobilsk�rmen
            // den returnerar tillbaks positionen vart jag tryckte
            // n�gonstans d�r och kopierar �ver v�rdet till touchPos
            touchPos = Input.GetTouch(0).position;
            
            return true;
        }

        touchPos = default; // L�gg m�rke till nyckelordet "default"

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
            // F� ut information 
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
