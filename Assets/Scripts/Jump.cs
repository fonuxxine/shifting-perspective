using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;

    private float _playerInput;
    private float _rotationInput;
    private bool _userJumped;

    private Vector3 _userRot;
    private const float _inputScale = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerInput = Input.GetAxis("Vertical");
        _rotationInput = Input.GetAxis("Horizontal");
        _userJumped = Input.GetButton("Jump");
    }
    
    private void FixedUpdate()
    {
        _userRot = _transform.rotation.eulerAngles;
        _userRot += new Vector3(0, _rotationInput, 0);
        _transform.rotation = Quaternion.Euler(_userRot);
        _rigidbody.velocity += transform.forward * _playerInput * _inputScale;

        if (_userJumped)
        {
            _rigidbody.AddForce(Vector3.up, ForceMode.VelocityChange);
            _userJumped = false;
        }
    }
}
