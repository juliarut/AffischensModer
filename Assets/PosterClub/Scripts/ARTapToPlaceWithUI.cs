using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// NYTT: Använder en enum för att hålla reda på vilken sida som visas etc
/// </summary>
public enum PageState
{
    TRACKING,
    PLACE,
    MAIN,
}

public class ARTapToPlaceWithUI : MonoBehaviour
{
    [SerializeField]
    private GameObject refToPrefab;

    [SerializeField]
    private ARRaycastManager raycastManager;

    [SerializeField]
    private ARPlaneManager planeManager;

    [SerializeField]
    private GameObject referenceToCanvas;

    private static List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    private GameObject spawnedObject;

    private bool hasBeenCreated = false;

    /// <summary>
    /// NYTT: Visa nuvarande tillståndet för vilken sida som visas
    /// </summary>
    private PageState currentPageState;

    // NYTT: En lista av Olika "sidor" (pages) som vi ska visa vid olika tidpunkter samt dess transform
    private Dictionary<string, Transform> _listOfPages = new Dictionary<string, Transform>();

    private void Start()
    {
        ///// NYTT /////////////////////
        // Gå igenom listan av alla childobjekt till referenceToCanvas objektet i hiearchy
        // och spar dem till dictionarylistan _listOfPages
        foreach(Transform t in referenceToCanvas.transform)
        {
            _listOfPages.Add(t.gameObject.name, t);

            // Se till att vi alltid startar med att alla "sidor" (pages) är dolda
            t.gameObject.SetActive(false);
        }

        ///// NYTT /////////////////////
        // Börja (exempelvis om nu eran app ska vara sådan) att
        // visa att man ska scanna av ytan med mobilkameran
        ChangePage("OnBoardingTrackingPage");

        ///// NYTT /////////////////////
        currentPageState = PageState.TRACKING; // Sätt också att "läget" är i trackingmode
    }


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

        ///// NYTT /////////////////////
        /// Denna del läser av från planeManager om vi lyckats hitta några plane  
        /// Samt kolla också så vi befann oss i tracking läget
        if (planeManager.trackables.count > 0 && currentPageState == PageState.TRACKING)
        {
            // Växla då sida till att börja placera ut objektet
            ChangePage("OnBoardingPlacePage");
            currentPageState = PageState.PLACE; // byte läge till att placera ut
        }

        if (raycastManager.Raycast(touchPos, hitResults, TrackableType.Planes) && currentPageState == PageState.PLACE)
        {
            // Få ut information 
            Pose hitPose = hitResults[0].pose;

            ///// NYTT /////////////////////
            // Växla nu till sista rutan
            ChangePage("MainPage");
            currentPageState = PageState.MAIN;

            ///// ÖVNING ///////////////////
            // Skriv här hur man då döljer planen efter man placerat ut

            if (spawnedObject == null && hasBeenCreated == false)
            {
                spawnedObject = Instantiate(refToPrefab, hitPose.position, hitPose.rotation);
                hasBeenCreated = true;
            }
        }
    }

    /// <summary>
    /// ///// NYTT /////////////////////
    /// Denna funktion är till för att förenkla att dölja tidigare sidor
    /// och visa den nya sidan istället som är relevant i sammanhanget
    /// </summary>
    /// <param name="newPageName"></param>
    private void ChangePage(string newPageName)
    {
        // Gå igenom alla sidor och dölj dessa först
        foreach(KeyValuePair<string, Transform> page in _listOfPages)
        {
            // om vi hittade den nya sidan så visa just den
            if (page.Key == newPageName)
            {
                page.Value.gameObject.SetActive(true);
            } else
            {
                // Dölj resten av sidorna
                page.Value.gameObject.SetActive(false);
            }
        }
    }

}
