using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CharacterController _characterController;
    
    private Vector2 _currMovementInput;
    private Vector3 _currMovement;

    private bool _isJumpPressed = false;
    private float _initialJumpVelocity;
    private float _maxJumpHeight = 4.0f;
    private float _maxJumpTime = 0.75f;
    private bool _isJumping;
    
    private float _gravity = -9.8f;
    private float _groundedGravity = -.02f;
    

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        
        _playerInput.CharacterControls.Jump.started += OnJump;
        _playerInput.CharacterControls.Jump.canceled += OnJump;
        

        JumpVariables();
    }

    void JumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currMovementInput = context.ReadValue<Vector2>();
        _currMovement.x = _currMovementInput.x;
        _currMovement.z = _currMovementInput.y;
    }
    
    void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        
    }

    void HandleJump()
    {
        if (!_isJumping && _characterController.isGrounded && _isJumpPressed)
        {
            _isJumping = true;
            _currMovement.y = _initialJumpVelocity * .5f;
        }
        else if (_isJumping && _characterController.isGrounded && !_isJumpPressed)
        {
            _isJumping = false;
        }
    }

    void HandleGravity()
    {
        bool isFalling = _currMovement.y <= 0.0f || !_isJumpPressed;
        float fallMult = 2.0f;
        
        if (_characterController.isGrounded)
        {
            _currMovement.y = _groundedGravity;
        } else if (isFalling)
        {
            float prevYVelocity = _currMovement.y;
            float newYVelocity = _currMovement.y + (_gravity * fallMult * Time.deltaTime);
            float nextYVelocity = (prevYVelocity + newYVelocity) * .5f;
            _currMovement.y = nextYVelocity;
        }
        else
        {
            float prevYVelocity = _currMovement.y;
            float newYVelocity = _currMovement.y + (_gravity * Time.deltaTime);
            float nextYVelocity = (prevYVelocity + newYVelocity) * .5f;
            _currMovement.y = nextYVelocity;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        _characterController.Move(_currMovement * Time.deltaTime);
        HandleGravity();
        HandleJump();
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }
}
