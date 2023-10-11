using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneController : MonoBehaviour
{
    [SerializeField]
    private ARPlaneManager planeManager;

    // Funktion för att växla mellan att visa och dölja alla plan som har trackats
    public void TogglePlaneVisibility()
    {
        // Kontrollera att planeManager är inställd och inte null
        if (planeManager != null)
        {
            // Växla planeManager mellan aktiverad och inaktiverad
            planeManager.enabled = !planeManager.enabled;

            // För varje plan i planeManager-trackables, aktivera eller inaktivera dess GameObject
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(planeManager.enabled);
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
}