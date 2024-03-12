using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class rotateExt : MonoBehaviour
{
    public float _userRotateYInput;
    private Transform objectTransform;
    private float currentYAngle;
    private Quaternion baseAngles;
    private bool rotationReset = true;
    public bool rotating = false;
    public bool stationaryPlatform = true;
    
    private cameraRotate cameraScript;
    public string cameraObjectName;

    //  // Variables for wall transparency...
    // public GameObject wall0, wall1, wall2, wall3;
    // private GameObject currentFacingWall = null;
    // private Material originalMaterial;
    // public Material transparentMaterial;
    
    
    // Start is called before the first frame update
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>();
        currentYAngle = objectTransform.rotation.eulerAngles.y;
        baseAngles = objectTransform.rotation;
        // originalMaterial = DetermineFacingWall().GetComponent<Renderer>().material;
        
        if (!string.IsNullOrEmpty(cameraObjectName))
        {
            cameraScript = GameObject.Find(cameraObjectName).GetComponent<cameraRotate>();
        }
        else
        {
            Debug.LogError("Please assign the parent GameObject's name in the Unity Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isCameraReset = (cameraScript != null) ? cameraScript.rotationReset : false;
        if (!isCameraReset)
        {
            rotationReset = false;
        }
        if (_userRotateYInput != 0 || !rotationReset) 
        {
            rotating = true;
        } else
        {
            rotating = false;
        }
        HandleRotationInput();
        
        // UpdateWallTransparency();
    }


     private void HandleRotationInput()
    {
        if (rotationReset)
        {
            _userRotateYInput = Input.GetAxis("RotateHori");
        }
        else
        {
            _userRotateYInput = 0;
            if (Input.GetAxis("RotateHori") == 0)
            {
                rotationReset = true;
            }
        }
        
    }

    // update the level used as the default/base angle for this portion of the level
    public void UpdateBaseAngles(Quaternion angles)
    {
        baseAngles = angles;
    }

    // reset the rotation of the level to the base rotation for this portion of the stage
    public void ResetRotation()
    {
        cameraScript.ResetRotation(baseAngles.eulerAngles.y);
    }
}
