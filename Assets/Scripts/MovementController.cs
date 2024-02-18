using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private Animator _animator;
    
    private Vector2 _currMovementInput;
    private Vector3 _currMovement;

    private bool _isMovementPressed = false;
    public float _moveFactor = 1.0f;
    public float _rotationFactor = 2.0f;
    
    private bool _isJumpPressed = false;
    private float _initialJumpVelocity;
    // modify this value for larger Jump height
    public float _maxJumpHeight = 4.0f;
    // modify this value for longer Jump time
    public float _maxJumpTime = 0.75f;
    private bool _isJumping;
    
    private float _gravity = -9.8f;
    private float _groundedGravity = -1f;
    
    
    
    Collider _playerCollider;
    

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        
        _playerInput.CharacterControls.Jump.started += OnJumpInput;
        _playerInput.CharacterControls.Jump.canceled += OnJumpInput;
        
        _playerCollider = GetComponent<Collider>();
        
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
        _isMovementPressed = _currMovementInput.x != 0 | _currMovementInput.y != 0;
    }
    
    void OnJumpInput(InputAction.CallbackContext context)
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
        else if (!_isJumpPressed && _characterController.isGrounded && _isJumping)
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
        
        // bool isFalling =  _currMovement.y <= 0.0f || !_isJumping;
        // float fallMult = 2.0f;
        //
        // float distToMoveDown = 9.81f * Time.deltaTime;
        // RaycastHit hit;
        // Vector3 origin = transform.position -
        //                  new Vector3(0, _characterController.center.y + _characterController.height + .2f, 0);
        // _isGrounded = Physics.Raycast(origin, Vector3.down, out hit, _playerCollider.bounds.extents.y,
        //     LayerMask.GetMask(("Floor")));
        // if (_isGrounded) 
        // {
        //     distToMoveDown = hit.distance;
        //     _characterController.Move(new Vector3(0, -distToMoveDown, 0));
        //     Debug.Log("isGrounded");
        // } else if (isFalling)
        // {
        //     float prevYVelocity = _currMovement.y;
        //     float newYVelocity = _currMovement.y + (_gravity * fallMult * Time.deltaTime);
        //     float nextYVelocity = (prevYVelocity + newYVelocity) * .5f;
        //     _currMovement.y = nextYVelocity;
        //     Debug.Log("isFalling");
        // }
        // else
        // {
        //     float prevYVelocity = _currMovement.y;
        //     float newYVelocity = _currMovement.y + (_gravity * Time.deltaTime);
        //     float nextYVelocity = (prevYVelocity + newYVelocity) * .5f;
        //     _currMovement.y = nextYVelocity;
        // }
        
        //
        //
        // if (_characterController.isGrounded)
        // {
        //     _currMovement.y = _groundedGravity;
        // } else if (_isFalling)
        // {
        //     float prevYVelocity = _currMovement.y;
        //     float newYVelocity = _currMovement.y + (_gravity * fallMult * Time.deltaTime);
        //     float nextYVelocity = (prevYVelocity + newYVelocity) * .5f;
        //     _currMovement.y = nextYVelocity;
        // }
    }

    void handleAnimation()
    {
        bool isWalking = _animator.GetBool("isWalking");
        bool isJumping = _animator.GetBool("isJumping");

        if (_isMovementPressed && !isWalking)
        {
            _animator.SetBool("isWalking", true);
        }

        if (!_isMovementPressed && isWalking)
        {
            _animator.SetBool("isWalking", false);
        }

        if (!isJumping && _isJumpPressed)
        {
            _animator.SetBool("isJumping", true);
        }
        if (isJumping && !_isJumpPressed)
        {
            _animator.SetBool("isJumping", false);
        }
    }


    void handleRotation()
    {
        Vector3 direction;

        direction.x = _currMovement.x;
        direction.y = 0.0f;
        direction.z = _currMovement.z;

        Quaternion currDirection = transform.rotation;
        if (_isMovementPressed)
        {
            Quaternion targetDirection = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(currDirection, targetDirection, _rotationFactor * Time.deltaTime);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        handleAnimation();
        handleRotation();
        _characterController.Move(_currMovement * Time.deltaTime * _moveFactor);
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
