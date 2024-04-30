using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefabObject;
    public int objectNumberOnStart;

    private List<GameObject> poolObjects = new List<GameObject>();

    private void Start()
    {
        //create the object needed at the begining of the game
        for(int i = 0; i < objectNumberOnStart; i++)
        {
            CreateNewObject();
        }
    }

    /// <summary>
    /// Instantiate new object and added to the List
    /// </summary>
    /// <returns>a GameObject</returns>
    private GameObject CreateNewObject()
    {
            GameObject gameObject = Instantiate(prefabObject);
            gameObject.SetActive(false);
            poolObjects.Add(gameObject);

            return gameObject;
    }
    /// <summary>
    /// Take from the list an available object if not 
    /// exist create a new one
    /// </summary>
    /// <returns>a object</returns>
    public GameObject GetGameObject()
    {
        //find in the poolObject an object that is inactive in the 
        //game herarchy 
        GameObject gameObject = poolObjects.Find(x => x.activeInHierarchy == false);
        
        //if non exist create one
        if (gameObject == null)
        {
            gameObject = CreateNewObject();
        }

        gameObject.SetActive(true);

        return gameObject;
    }
}
