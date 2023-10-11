using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// NYTT: Anv�nder en enum f�r att h�lla reda p� vilken sida som visas etc
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
    /// NYTT: Visa nuvarande tillst�ndet f�r vilken sida som visas
    /// </summary>
    private PageState currentPageState;

    // NYTT: En lista av Olika "sidor" (pages) som vi ska visa vid olika tidpunkter samt dess transform
    private Dictionary<string, Transform> _listOfPages = new Dictionary<string, Transform>();

    private void Start()
    {
        ///// NYTT /////////////////////
        // G� igenom listan av alla childobjekt till referenceToCanvas objektet i hiearchy
        // och spar dem till dictionarylistan _listOfPages
        foreach(Transform t in referenceToCanvas.transform)
        {
            _listOfPages.Add(t.gameObject.name, t);

            // Se till att vi alltid startar med att alla "sidor" (pages) �r dolda
            t.gameObject.SetActive(false);
        }

        ///// NYTT /////////////////////
        // B�rja (exempelvis om nu eran app ska vara s�dan) att
        // visa att man ska scanna av ytan med mobilkameran
        ChangePage("OnBoardingTrackingPage");

        ///// NYTT /////////////////////
        currentPageState = PageState.TRACKING; // S�tt ocks� att "l�get" �r i trackingmode
    }


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

        ///// NYTT /////////////////////
        /// Denna del l�ser av fr�n planeManager om vi lyckats hitta n�gra plane  
        /// Samt kolla ocks� s� vi befann oss i tracking l�get
        if (planeManager.trackables.count > 0 && currentPageState == PageState.TRACKING)
        {
            // V�xla d� sida till att b�rja placera ut objektet
            ChangePage("OnBoardingPlacePage");
            currentPageState = PageState.PLACE; // byte l�ge till att placera ut
        }

        if (raycastManager.Raycast(touchPos, hitResults, TrackableType.Planes) && currentPageState == PageState.PLACE)
        {
            // F� ut information 
            Pose hitPose = hitResults[0].pose;

            ///// NYTT /////////////////////
            // V�xla nu till sista rutan
            ChangePage("MainPage");
            currentPageState = PageState.MAIN;

            ///// �VNING ///////////////////
            // Skriv h�r hur man d� d�ljer planen efter man placerat ut

            if (spawnedObject == null && hasBeenCreated == false)
            {
                spawnedObject = Instantiate(refToPrefab, hitPose.position, hitPose.rotation);
                hasBeenCreated = true;
            }
        }
    }

    /// <summary>
    /// ///// NYTT /////////////////////
    /// Denna funktion �r till f�r att f�renkla att d�lja tidigare sidor
    /// och visa den nya sidan ist�llet som �r relevant i sammanhanget
    /// </summary>
    /// <param name="newPageName"></param>
    private void ChangePage(string newPageName)
    {
        // G� igenom alla sidor och d�lj dessa f�rst
        foreach(KeyValuePair<string, Transform> page in _listOfPages)
        {
            // om vi hittade den nya sidan s� visa just den
            if (page.Key == newPageName)
            {
                page.Value.gameObject.SetActive(true);
            } else
            {
                // D�lj resten av sidorna
                page.Value.gameObject.SetActive(false);
            }
        }
    }

}
