using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float movementModifier = 1f; // multiplied with the base movement speed for times when it must be adjusted
    
    private float _userHorizontalInput;
    private const float ScaleMovement = 0.005f;

    private Transform playerTransform;
    
    private float _userRotInput;
    private Vector3 _userRot;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = gameObject.GetComponent<Transform>();   
    }

    // Update is called once per frame
    void Update()
    {
        _userHorizontalInput = Input.GetAxis("Vertical");
        _userRotInput = Input.GetAxis("Horizontal");
        
        _userRot = new Vector3(_userRotInput, 0, _userHorizontalInput);
        
        playerTransform.position += _userRot * (ScaleMovement * movementModifier);
    }
}