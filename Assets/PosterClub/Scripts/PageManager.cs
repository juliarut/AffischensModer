using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public GameObject mainPage;
    public GameObject scanWallPage;
    public ARPlaneManager planeManager;
    public GameObject scanIcon;

    private void Start()
    {
        // Inaktivera Scan Wall Page och scanIcon när applikationen börjar
        scanWallPage.SetActive(false);
        scanIcon.SetActive(false);
    }

    public void StartScanning()
    {
        // Aktivera Scan Wall Page
        mainPage.SetActive(false);
        scanWallPage.SetActive(true);

        // Aktivera AR Plane tracking
        planeManager.enabled = true;

        // Visa scanIcon
        scanIcon.SetActive(true);
    }
}

