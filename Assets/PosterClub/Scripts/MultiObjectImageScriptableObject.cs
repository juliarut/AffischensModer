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
    /// Initierar all data som behövs för att binda
    /// bildreferensnamnet i bilddatabasen med något gameobjekt
    /// </summary>
    public void Init()
    {
        foreach (var arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name; // kopiera över namnet från objektet till namnet på kopian
            newARObject.SetActive(false); // dölj kopian
            arObjects.Add(arObject.name, newARObject); // För över allt nu (dvs kopian) till dictionary
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
