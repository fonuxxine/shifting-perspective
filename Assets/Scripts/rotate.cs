using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class rotate : MonoBehaviour
{
    public float _userRotateYInput;
    public float _userRotateXInput;
    private Transform objectTransform;
    private float currentYAngle;
    public float currentXAngle;
    private Quaternion baseAngles;
    private bool rotationReset = true;
    public bool rotating = false;
    public bool takingInput = true;
    
    private cameraRotate cameraScript;
    public string cameraObjectName;
    public bool ext = false;
    // check if character is grounded
    private bool _grounded = true;
    
    public AudioClip rotateSound;
    private AudioSource _audioSource;

    public int rotations = 0;
    public int cameraRotations = 0;
    
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
        rotating = !rotationReset || !isCameraReset;

        if (!takingInput && Input.GetAxis("RotateHori") == 0 && Input.GetAxis("RotateVerti") == 0)
        {
            takingInput = true;
        }

        if (rotating)
        {
            takingInput = false;
            _userRotateYInput = 0;
            _userRotateXInput = 0;
        }

        if (takingInput)
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
        HandleRotationInput();
        
    }
    
    IEnumerator RotateVerti(float angle, float duration=1f)
    {
        if (_audioSource && rotateSound) {
            _audioSource.PlayOneShot(rotateSound, 0.15f);
        }
        
        Quaternion startRotation = Quaternion.Euler(0, currentYAngle, currentXAngle);
        Quaternion endRotation = Quaternion.Euler(0, currentYAngle, currentXAngle + angle);
        float timePassed = 0f;
        while (timePassed < duration)
        {
            rotationReset = false;
            objectTransform.rotation = Quaternion.Slerp(startRotation, endRotation, timePassed / duration);
            timePassed += Time.deltaTime;
            yield return null;
        }
        currentXAngle = endRotation.eulerAngles.z;
        objectTransform.rotation = endRotation; // Ensure the final rotation is set
        rotationReset = true;
        rotations += 1;
        // Debug.Log("RotateVerti complete");
    }


     private void HandleRotationInput()
    {
        if (!ext)
        {
            if (rotationReset)
            {
                if (_userRotateXInput > 0)
                {
                    takingInput = false;
                    _userRotateYInput = 0;
                    _userRotateXInput = 0;
                    StartCoroutine(RotateVerti(90f));
                }
                else if (_userRotateXInput < 0)
                {
                    takingInput = false;
                    _userRotateYInput = 0;
                    _userRotateXInput = 0;
                    StartCoroutine(RotateVerti(-90f));
                }
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
