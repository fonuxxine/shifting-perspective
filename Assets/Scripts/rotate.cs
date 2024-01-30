using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class rotate : MonoBehaviour
{
    public float _userRotateYInput;
    public float _userRotateXInput;
    private Transform objectTransform;
    private float currentYAngle = 0f;
    private float currentXAngle = 0f;
    private bool rotationReset = true;

     // Variables for wall transparency...
    public GameObject wall0, wall1, wall2, wall3;
    private GameObject currentFacingWall = null;
    private Material originalMaterial;
    public Material transparentMaterial;
    
    
    // Start is called before the first frame update
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>();

        originalMaterial = DetermineFacingWall().GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotationInput();
        
        UpdateWallTransparency();
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
        Vector3 forward = objectTransform.forward;
        if (Mathf.Abs(forward.z) > Mathf.Abs(forward.x))
        {
            // Facing North or South
            return forward.z > 0 ? wall0 : wall2;
        }
        else
        {
            // Facing East or West
            return forward.x > 0 ? wall3 : wall1;
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
