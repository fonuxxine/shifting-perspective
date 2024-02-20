using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stationaryPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    private rotate rotateScript;
    public string parentGameObjectName;
    void Start()
    {
        if (!string.IsNullOrEmpty(parentGameObjectName))
        {
            rotateScript = GameObject.Find(parentGameObjectName).GetComponent<rotate>();
        }
        else
        {
            Debug.LogError("Please assign the parent GameObject's name in the Unity Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isStationary = (rotateScript != null) ? rotateScript.stationaryPlatform : true;
        if (isStationary)
        {
            transform.parent = GameObject.Find(parentGameObjectName).transform;
        }
        else
        {
            transform.parent = null;
        }
    }
}
