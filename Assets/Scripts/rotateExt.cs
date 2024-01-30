using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class rotateExt : MonoBehaviour
{
    private float _userRotateYInput;
    private Transform objectTransform;
    private float currentYAngle = 0f;
    private bool rotationReset = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
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

        if (_userRotateYInput != 0)
        {
            if (Math.Abs(_userRotateYInput) < 1)
            {
                objectTransform.rotation = Quaternion.Euler(0, currentYAngle + _userRotateYInput * 10f, 0);
            } else if (_userRotateYInput == 1)
            {
                currentYAngle += 10f;
                StartCoroutine(Rotate(80f));
            } else if (_userRotateYInput == -1)
            { 
                currentYAngle -= 10f;
                StartCoroutine(Rotate(-80f));
            }
        } 
    }

    IEnumerator Rotate(float angle)
    {
        float duration = 1f;
        Quaternion startRotation = Quaternion.Euler(0, currentYAngle, 0);
        Quaternion endRotation = Quaternion.Euler(0, currentYAngle + angle, 0);
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
}
