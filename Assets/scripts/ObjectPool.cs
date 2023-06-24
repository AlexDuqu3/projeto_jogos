using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject[] objectPrefabs;

    public GameObject GetObject(string type)
    {
        foreach(GameObject prefab in objectPrefabs)
        {
            if(prefab.name == type)
            {
                GameObject newObject = Instantiate(prefab);
                newObject.name = type;
                return newObject;
            }
        }
        return null;
    }
}
