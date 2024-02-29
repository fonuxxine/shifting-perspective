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
    public enum rotationStates { faceFront = 0, faceLeft = 1, faceBack = 2, faceRight = 3 };
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
        float _userRotateYInput = (rotateScript != null) ? -rotateScript._userRotateYInput : 0f;
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

    IEnumerator RotateHori(float angle, float duration=1f)
    {
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
        print("Now Rotation State: " + currentRotationState.ToString());

        // map the currentRotationState enum to an integer
        int currentStateValue = (int) currentRotationState;

        // calculate the adjusted quarter rotations based on the current rotation state
        int quarterRotations = Mathf.RoundToInt(rotAngle / 90f + currentStateValue) % 4;

        // shift negative quarter rotations
        if (quarterRotations < 0)
        {
            quarterRotations += 4;
        }
        
        // set new rotation state
        if (quarterRotations is >= 0 and < 4)
        {
            currentRotationState = (rotationStates) quarterRotations;
            // print("New Rotation State: " + currentRotationState.ToString());
        }
        // else
        // {
        //     print("Old Rotation State: " + currentRotationState.ToString());
        // }
    }
    
    public void ResetRotation(Quaternion baseAngles)
    {
        // Debug.Log("Current Angle Y: " + currentYAngle);
        // Debug.Log("Target Angle Y: " + (baseAngles.eulerAngles.y - 180)); // TODO add parameter to CheckpointHit for this 180 modifier
        float diff = Mathf.DeltaAngle(currentYAngle, baseAngles.eulerAngles.y - 180);
        // Debug.Log("Calculated Diff: " + diff);

        float duration = diff == 0 ? 0 : 0.75f;
        StartCoroutine(RotateHori(diff, duration:duration));
    }

}
