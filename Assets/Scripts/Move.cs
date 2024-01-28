using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float _userHorizontalInput;
    private const float ScaleMovement = 0.1f;

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
        
        playerTransform.position += _userRot * ScaleMovement;
    }
}
