using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MultiImageObject", menuName = "ScriptableObjects/MultiImageObject")]
public class MultiObjectImageScriptableObject : ScriptableObject
{
    [SerializeField]
    private List<GameObject> arObjectsToPlace = new List<GameObject>();

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    /// <summary>
    /// Initierar all data som beh�vs f�r att binda
    /// bildreferensnamnet i bilddatabasen med n�got gameobjekt
    /// </summary>
    public void Init()
    {
        foreach (var arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name; // kopiera �ver namnet fr�n objektet till namnet p� kopian
            newARObject.SetActive(false); // d�lj kopian
            arObjects.Add(arObject.name, newARObject); // F�r �ver allt nu (dvs kopian) till dictionary
        }
    }

    public GameObject GetObjectByImageName(string imageName)
    {
        if (arObjects.ContainsKey(imageName))
        {
            return arObjects[imageName];
        } else
        {
            return null;
        }
    }

    public void UpdatePositionRotation(string imageName, Transform newTransform)
    {
        if (arObjects.ContainsKey(imageName))
        {
            arObjects[imageName].transform.position = newTransform.position;
            arObjects[imageName].transform.rotation = newTransform.rotation;
        }
    }

    public void ChangeActive(string imageName, bool isActive)
    {
        if (arObjects.ContainsKey(imageName))
        {
            arObjects[imageName].SetActive(isActive);
        }
    }

}
