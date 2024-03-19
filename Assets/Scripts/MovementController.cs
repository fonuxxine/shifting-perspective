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
    public float _moveFactor = 1.5f;
    public float _rotationFactor = 2.0f;
    
    private bool _isJumpPressed = false;
    private float _initialJumpVelocity;
    // modify this value for larger Jump height
    public float _maxJumpHeight = 4.0f;
    // modify this value for longer Jump time
    public float _maxJumpTime = 0.75f;
    private bool _isJumping;
    private float _lastGroundedTime;
    
    private float _coyoteTime = .2f;
    private float _onGround = float.MinValue;
    public AudioClip jumpSound;
    private AudioSource _audioSource;
    
    // handle fall damage
    private float _timeFalling = 0f;
    private bool _damage = false;
    private PlayerHealth _playerHealth;
    
    private float _gravity = -9.8f;
    private float _groundedGravity = -1f;
    private rotate rotateScript;
    public string parentGameObjectName;
    private cameraRotate cameraScript;
    public string cameraObjectName;
    private 
    
    Collider _playerCollider;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();
        

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Jump.started += OnJumpInput;
        _playerInput.CharacterControls.Jump.canceled += OnJumpInput;
        _playerCollider = GetComponent<Collider>();
        
        if (!string.IsNullOrEmpty(parentGameObjectName)) //get level rotation script
        {
            rotateScript = GameObject.Find(parentGameObjectName).GetComponent<rotate>();
        }
        else
        {
            Debug.LogError("Please assign the parent GameObject's name in the Unity Inspector.");
        }
        
        if (!string.IsNullOrEmpty(cameraObjectName)) //get camera rotation script
        {
            cameraScript = GameObject.Find(cameraObjectName).GetComponent<cameraRotate>();
        }
        else
        {
            Debug.LogError("Please assign the parent GameObject's name in the Unity Inspector.");
        }

        _audioSource = GetComponentInChildren<AudioSource>();
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
        //check camera rotation state to determine movement direction
        if (cameraScript != null)
        {
            if (cameraScript.currentRotationState == cameraRotate.rotationStates.faceFront)
            {
                _currMovementInput = context.ReadValue<Vector2>();
            }
            else if (cameraScript.currentRotationState == cameraRotate.rotationStates.faceLeft)
            {
                _currMovementInput = new Vector2(context.ReadValue<Vector2>().y, -context.ReadValue<Vector2>().x);
            }
            else if (cameraScript.currentRotationState == cameraRotate.rotationStates.faceBack)
            {
                _currMovementInput = new Vector2(-context.ReadValue<Vector2>().x, -context.ReadValue<Vector2>().y);
            }
            else if (cameraScript.currentRotationState == cameraRotate.rotationStates.faceRight)
            {
                _currMovementInput = new Vector2(-context.ReadValue<Vector2>().y, context.ReadValue<Vector2>().x);
            }
        }
        
        _currMovement.x = _currMovementInput.x * _moveFactor;
        _currMovement.z = _currMovementInput.y * _moveFactor;
        _isMovementPressed = _currMovementInput.x != 0 | _currMovementInput.y != 0;
    }
    
    void OnJumpInput(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }

    void HandleJump()
    {
        if (!_isJumping && (_characterController.isGrounded || Time.time - _onGround < _coyoteTime) && _isJumpPressed)
        {
            _isJumping = true;
            _currMovement.y = _initialJumpVelocity * .5f;
            _lastGroundedTime = Time.time;

            if (_audioSource && jumpSound) {
                _audioSource.PlayOneShot(jumpSound, 0.055F);
            }
            
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
            if (_timeFalling > 0.6f)
            {
                _damage = true;
            }
            _timeFalling = 0f;
            _onGround = Time.time;
            rotateScript.AllowRotation();
        } else if (isFalling)
        {
            float prevYVelocity = _currMovement.y;
            float newYVelocity = _currMovement.y + (_gravity * fallMult * Time.deltaTime);
            float nextYVelocity = (prevYVelocity + newYVelocity) * .5f;
            _currMovement.y = nextYVelocity;
            _timeFalling += Time.deltaTime;
            
        }
        else
        {
            float prevYVelocity = _currMovement.y;
            float newYVelocity = _currMovement.y + (_gravity * Time.deltaTime);
            float nextYVelocity = (prevYVelocity + newYVelocity) * .5f;
            _currMovement.y = nextYVelocity;
        }
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

        if (Time.time - _lastGroundedTime > _maxJumpTime && Time.time - _lastGroundedTime < _maxJumpTime + 0.2f)
        {
            _animator.SetBool("isLanding", true);
        }
        else
        {
            _animator.SetBool("isLanding", false);
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

    void handleRotateLevel()
    {
        Quaternion currDirection = transform.rotation;
        
        float rotateX = (rotateScript != null) ? rotateScript.rotateX : 0f;
        Quaternion targetDirection = Quaternion.Euler(
            currDirection.eulerAngles.x + 2*rotateX,
            currDirection.eulerAngles.y, 
            currDirection.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(currDirection, targetDirection, _rotationFactor * Time.deltaTime);
    }
    
    // Update is called once per frame
    void Update()
    {
        bool isRotating = (rotateScript != null) ? rotateScript.rotating : false;
        if (!isRotating)
        {
            _animator.enabled = true;
            JumpVariables();
            handleAnimation();
            handleRotation();
            HandleGravity();
            HandleJump();
            _characterController.Move(_currMovement * Time.deltaTime);
        }
        else
        {
            //handleRotateLevel();
            _animator.enabled = false;
        }
        if (_damage)
        {
            _playerHealth.DecreaseHealth(1);
            _damage = false;
        }
        // if (!_characterController.isGrounded)
        // {
        //     rotateScript.SetRotationInput();
        // }
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
