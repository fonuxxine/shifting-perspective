using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class rotate : MonoBehaviour
{
    public float _userRotateYInput;
    public float _userRotateXInput;
    public float rotateX = 0f;
    private Transform objectTransform;
    private float currentYAngle;
    private float currentXAngle;
    private Quaternion baseAngles;
    private bool rotationReset = true;
    public bool rotating = false;
    public bool stationaryPlatform = true;
    
    private cameraRotate cameraScript;
    public string cameraObjectName;
    public bool ext = false;
    // check if character is grounded
    private bool _grounded = true;
    
    public AudioClip rotateSound;
    private AudioSource _audioSource;
    
    
    // Start is called before the first frame update
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>();
        currentYAngle = objectTransform.rotation.eulerAngles.y;
        currentXAngle = objectTransform.rotation.eulerAngles.x;
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
        
        _audioSource = GetComponentInChildren<AudioSource>();

        if (!_audioSource)
        {
            Debug.LogWarning("Level has no AudioSource.");
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
        if (_userRotateYInput != 0 || _userRotateXInput != 0 || !rotationReset) 
        {
            rotating = true;
        } else
        {
            rotating = false;
        }

        if (_grounded)
        {
            HandleRotationInput();
        }
        
    }
    
    IEnumerator RotateVerti(float angle, float duration=1f)
    {
        if (_audioSource && rotateSound) {
            _audioSource.PlayOneShot(rotateSound, 0.035f);
        }
        
        Quaternion startRotation = Quaternion.Euler(0, currentYAngle, currentXAngle);
        Quaternion endRotation = Quaternion.Euler(0, currentYAngle, currentXAngle + angle);
        float timePassed = 0f;
        while (timePassed < duration)
        {
            stationaryPlatform = false;
            rotationReset = false;
            objectTransform.rotation = Quaternion.Slerp(startRotation, endRotation, timePassed / duration);
            timePassed += Time.deltaTime;
            yield return null;
        }
        currentXAngle += angle;
        rotateX = 0f;
        stationaryPlatform = true;
        objectTransform.rotation = endRotation; // Ensure the final rotation is set
        
        // Debug.Log("RotateVerti complete");
    }


     private void HandleRotationInput()
    {
        if (rotationReset)
        {
            if (_userRotateXInput == 0)
            {
                _userRotateYInput = Input.GetAxis("RotateHori");
            }
            if (_userRotateYInput == 0)
            {
                _userRotateXInput = Input.GetAxis("RotateVerti");
            } 
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
        
        if (!ext)
        {
            if (_userRotateXInput != 0)
            {
                stationaryPlatform = false;
                if (Math.Abs(_userRotateXInput) < 1)
                {
                    objectTransform.rotation =
                        Quaternion.Euler(0, currentYAngle, currentXAngle + _userRotateXInput * 10f);
                }
                else if (_userRotateXInput == 1)
                {
                    currentXAngle += 10f;
                    rotateX = 90f;
                    StartCoroutine(RotateVerti(80f));
                }
                else if (_userRotateXInput == -1)
                {
                    currentXAngle -= 10f;
                    rotateX = -90f;
                    StartCoroutine(RotateVerti(-80f));
                }
            }
            else
            {
                stationaryPlatform = true;
            }
        }
    }

    // update the level used as the default/base angle for this portion of the level
    public void UpdateBaseAngles(Quaternion angles)
    {
        baseAngles = angles;
    }

    // reset the rotation of the level to the base rotation for this portion of the stage
    public IEnumerator ResetRotation()
    {
        
        if (!ext)
        {
            cameraScript.ResetRotation(baseAngles.eulerAngles.y - 180);
            float diff = Mathf.DeltaAngle(currentXAngle, (baseAngles.eulerAngles.z >= 0 && baseAngles.eulerAngles.z <= 1) ? 0 : baseAngles.eulerAngles.z);
            float duration = diff == 0 ? 0 : 0.75f;
            AllowRotation();
            yield return StartCoroutine(RotateVerti(diff, duration:duration));
        }
        else
        {
            cameraScript.ResetRotation(baseAngles.eulerAngles.y + 90);
        }
        
        // Debug.Log("ResetRotation complete");
        
    }
    
    // set rotation input to 0 if character is not grounded
    public void SetRotationInput()
    {
        _grounded = false;
    }

    public void AllowRotation()
    {
        _grounded = true;
    }
}
