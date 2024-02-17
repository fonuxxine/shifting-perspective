using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class rotate2 : MonoBehaviour
{
    public float _userRotateYInput;
    public float _userRotateXInput;
    private Transform objectTransform;
    private float currentYAngle = 0f;
    private float currentXAngle = 0f;
    private bool rotationReset = true;

    public GameObject startWall; // This will be your 'wall0'
    public List<GameObject> leftWalls; // This will be your 'wall1' group
    public GameObject stopWall; // This will be your 'wall2'
    public List<GameObject> rightWalls; // This will be your 'wall3' group

     // Variables for wall transparency...
   
    private GameObject currentFacingWall = null;
    private Material originalMaterial;
    public Material transparentMaterial;
    
    
    // Start is called before the first frame update
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>();

    
        startWall = GameObject.Find("Start").gameObject; // Replace "Start" with the actual name in the hierarchy
        stopWall = GameObject.Find("Stop").gameObject; // Replace "Stop" with the actual name in the hierarchy

        Transform leftParent = GameObject.Find("Left").transform; // Replace "Left" with the actual parent name in the hierarchy
        Transform rightParent = GameObject.Find("Right").transform; 

        leftWalls = new List<GameObject>();
        rightWalls = new List<GameObject>();

        foreach (Transform child in leftParent)
        {
            leftWalls.Add(child.gameObject);
        }

        foreach (Transform child in rightParent)
        {
            rightWalls.Add(child.gameObject);
        }

        originalMaterial = DetermineFacingWall().GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotationInput();
        
        // UpdateWallTransparency();
    }

    IEnumerator RotateHori(float angle)
    {
        float duration = 1f;
        Quaternion startRotation = Quaternion.Euler(currentXAngle, currentYAngle, 0);
        Quaternion endRotation = Quaternion.Euler(currentXAngle, currentYAngle + angle, 0);
        float timePassed = 0f;
        while (timePassed < duration)
        {
            rotationReset = false;
            objectTransform.rotation = Quaternion.Slerp(startRotation, endRotation, timePassed / duration);
            timePassed += Time.deltaTime;
            yield return null;
        }
        currentYAngle += angle;
        objectTransform.rotation = endRotation; // Ensure the final rotation is set
    }
    
    IEnumerator RotateVerti(float angle)
    {
        float duration = 1f;
        Quaternion startRotation = Quaternion.Euler(currentXAngle, currentYAngle, 0);
        Quaternion endRotation = Quaternion.Euler(currentXAngle + angle, currentYAngle, 0);
        float timePassed = 0f;
        while (timePassed < duration)
        {
            rotationReset = false;
            objectTransform.rotation = Quaternion.Slerp(startRotation, endRotation, timePassed / duration);
            timePassed += Time.deltaTime;
            yield return null;
        }
        currentXAngle += angle;
        objectTransform.rotation = endRotation; // Ensure the final rotation is set
    }


     private void HandleRotationInput()
    {
        if (rotationReset)
        {
            _userRotateYInput = Input.GetAxis("RotateHori");
            _userRotateXInput = Input.GetAxis("RotateVerti");
        }
        else
        {
            _userRotateYInput = 0;
            _userRotateXInput = 0;
            if (Input.GetAxis("RotateHori") == 0 && Input.GetAxis("RotateVerti") == 0)
            {
                rotationReset = true;
            }
        }

        if (_userRotateYInput != 0)
        {
            if (Math.Abs(_userRotateYInput) < 1)
            {
                objectTransform.rotation = Quaternion.Euler(currentXAngle, currentYAngle + _userRotateYInput * 10f, 0);
            } else if (_userRotateYInput == 1)
            {
                currentYAngle += 10f;
                StartCoroutine(RotateHori(80f));
            } else if (_userRotateYInput == -1)
            { 
                currentYAngle -= 10f;
                StartCoroutine(RotateHori(-80f));
            }
        } else if (_userRotateXInput != 0)
        {
            if (Math.Abs(_userRotateXInput) < 1)
            {
                objectTransform.rotation = Quaternion.Euler(currentXAngle + _userRotateXInput * 10f, currentYAngle, 0);
            } else if (_userRotateXInput == 1)
            {
                currentXAngle += 10f;
                StartCoroutine(RotateVerti(80f));
            } else if (_userRotateXInput == -1)
            { 
                currentXAngle -= 10f;
                StartCoroutine(RotateVerti(-80f));
            }
        }
    }

    private void UpdateWallTransparency()
    {
        GameObject newFacingWall = DetermineFacingWall();
        if (currentFacingWall != newFacingWall)
        {
            // Revert the old wall to the original material
            if (currentFacingWall != null)
            {
                SetWallMaterial(currentFacingWall, originalMaterial);
            }

            // Update originalMaterial to the new wall's material before making it transparent
            originalMaterial = newFacingWall.GetComponent<Renderer>().material;

            // Make the new facing wall transparent
            SetWallMaterial(newFacingWall, transparentMaterial);

            currentFacingWall = newFacingWall;
        }
    }

    private GameObject DetermineFacingWall()
    {
        // Vector3 forward = objectTransform.forward;
        // if (Mathf.Abs(forward.z) > Mathf.Abs(forward.x))
        // {
        //     // Facing North or South
        //     return forward.z > 0 ? wall0 : wall2;
        // }
        // else
        // {
        //     // Facing East or West
        //     return forward.x > 0 ? wall3 : wall1;
        // }
         Vector3 forward = objectTransform.forward;
        if (forward.z > 0)
        {
            // Facing towards start wall
            return startWall;
        }
        else if (forward.z < 0)
        {
            // Facing towards stop wall
            return stopWall;
        }
        else if (forward.x > 0)
        {
            // Facing towards right walls
            return rightWalls.Count > 0 ? rightWalls[0] : null; // Return the first wall as representative
        }
        else
        {
            // Facing towards left walls
            return leftWalls.Count > 0 ? leftWalls[0] : null; // Return the first wall as representative
        }
    }

    private void SetWallMaterial(GameObject wall, Material material)
    {
        if (wall != null)
        {
            Renderer wallRenderer = wall.GetComponent<Renderer>();
            if (wallRenderer != null)
            {
                wallRenderer.material = material;
            }
        }
    }
}
