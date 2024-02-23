using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cameraRotate : MonoBehaviour
{ 
    private Transform objectTransform;
    private rotate rotateScript;
    public string parentGameObjectName;
    public bool rotationReset = true;
    public float currentYAngle;
    private float currentXAngle;
    public enum rotationStates {faceFront, faceLeft, faceBack, faceRight};
    public rotationStates currentRotationState;
    // Start is called before the first frame update
    void Start()
    {
        currentRotationState = rotationStates.faceFront;
        objectTransform = gameObject.GetComponent<Transform>();
        currentYAngle = objectTransform.rotation.eulerAngles.y;
        currentXAngle = objectTransform.rotation.eulerAngles.x;
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
        float _userRotateYInput = (rotateScript != null) ? rotateScript._userRotateYInput : 0f;
        bool levelRotating = (rotateScript != null) ? rotateScript.stationaryPlatform : false;
        if (levelRotating)
        {
            if (_userRotateYInput != 0)
            {
                if (Math.Abs(_userRotateYInput) < 1)
                {
                    objectTransform.rotation =
                        Quaternion.Euler(currentXAngle, currentYAngle + _userRotateYInput * 10f, 0);
                }
                else if (_userRotateYInput == 1)
                {
                    currentYAngle += 10f;
                    StartCoroutine(RotateHori(80f));
                }
                else if (_userRotateYInput == -1)
                {
                    currentYAngle -= 10f;
                    StartCoroutine(RotateHori(-80f));
                }
            }
        }
    }

    IEnumerator RotateHori(float angle)
    {
        float duration = 1f;
        Quaternion startRotation = Quaternion.Euler(currentXAngle, currentYAngle, 0);
        Quaternion endRotation = Quaternion.Euler(currentXAngle, currentYAngle + angle, 0);
        updateRotationState(angle);
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
        rotationReset = true;
    }
    
    void updateRotationState(float rotAngle)
    {
        if(currentRotationState == rotationStates.faceFront)
        {
            if(rotAngle == 80f)
            {
                currentRotationState = rotationStates.faceLeft;
            }
            else if(rotAngle == -80f)
            {
                currentRotationState = rotationStates.faceRight;
            }
        }
        else if(currentRotationState == rotationStates.faceLeft)
        {
            if(rotAngle == 80f)
            {
                currentRotationState = rotationStates.faceBack;
            }
            else if(rotAngle == -80f)
            {
                currentRotationState = rotationStates.faceFront;
            }
        }
        else if(currentRotationState == rotationStates.faceBack)
        {
            if(rotAngle == 80f)
            {
                currentRotationState = rotationStates.faceRight;
            }
            else if(rotAngle == -80f)
            {
                currentRotationState = rotationStates.faceLeft;
            }
        }
        else if(currentRotationState == rotationStates.faceRight)
        {
            if(rotAngle == 80f)
            {
                currentRotationState = rotationStates.faceFront;
            }
            else if(rotAngle == -80f)
            {
                currentRotationState = rotationStates.faceBack;
            }
        }
    }
}
