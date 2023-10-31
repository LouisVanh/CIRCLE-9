using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : MonoBehaviour
{
    
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _orientation;
    [SerializeField] private HealthBarUI _healthBar;
    private CharacterController _controller;
    private float _speed = 10f;
    private Vector3 _movement;
    private Vector3 _movementDirection;
    private Vector3 _moveDirection;
    public bool _isMoving = false;
    public bool _isSprinting= false;
    private float _horizontalInput;
    private float _verticalInput;
    public float _health;
    public float _maxHealth;
    public float _jumpGraceperiod;
    private float? _lastGroundedTime;
    private float? _jumpButtonPressedTime;
    [SerializeField] private float _mouseSensitivity = 2f;
    private float _cameraVerticalRotation = 0f;
    private float _gravity = -1f;
    private float _velocity;
    private float _horizontalSpeedMultiplier = 0.8f;
    [SerializeField] private float _jumpSpeed = 0.3f;
    private CameraHeadBob _headBob;

    void Start()
    {
        _healthBar.SetMaxHealth(_maxHealth);
        _headBob = GetComponentInChildren<CameraHeadBob>();
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Camera();
        Jumping();
        _horizontalInput = (Input.GetAxis("Horizontal") * _horizontalSpeedMultiplier);
        _verticalInput = Input.GetAxis("Vertical");
        Sprinting();

        Debug.Log(_health); 
        if(Input.GetKeyDown(KeyCode.K))
        {
            SetHealth(-20f);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SetHealth(+20f);
        }

        if(_controller.isGrounded)
        {
            _lastGroundedTime= Time.time;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _jumpButtonPressedTime= Time.time;
        }

    }
    public void SetHealth(float healthChange)
    {
        _health += healthChange;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _healthBar.SetHealth(_health);
    }

    private void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _isMoving)
        {
            _headBob.bobSpeed = 8;
            _headBob.bobAmount = 0.3f;

            _speed = 20f;
            _isSprinting = true;
            if (_camera.fieldOfView <= 90)
            {
                _camera.fieldOfView += 40 * Time.deltaTime;
            }
        }
        else
        {
            _headBob.bobSpeed = 6;
            _headBob.bobAmount = 0.15f;

            if (_camera.fieldOfView >= 80)
            {
                _camera.fieldOfView -= 40 * Time.deltaTime;
            }
            _isSprinting = false;
            _speed = 10f;
        }
    }
    private void FixedUpdate()
    {
        Movement();
        ApplyGravity();


        if (_moveDirection.sqrMagnitude > 0.2f && _controller.isGrounded) _isMoving = true;
        else { _isMoving = false; }
    }
    private void Camera()
    {
        float xInput = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float yInput = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _cameraVerticalRotation -= yInput;
        _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -90f, 90f);
        _camera.transform.localEulerAngles = Vector3.right * _cameraVerticalRotation;
        transform.Rotate(Vector3.up * xInput);
    }

    private void ApplyGravity()
    {
        if (!_controller.isGrounded)
        {
            _velocity += _gravity / 2 * Time.fixedDeltaTime;
        }
        else
        {
            _velocity = -0.1f;
        }
    }
    private void Movement()
    {

        _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
        Vector3 generalMovement = _moveDirection * _speed * Time.fixedDeltaTime;
        generalMovement.y = _velocity;
        _controller.Move(generalMovement);
    }
    private void Jumping()
    {

        if(Time.time - _lastGroundedTime <= _jumpGraceperiod)
        {
            if(Time.time - _jumpButtonPressedTime <= _jumpGraceperiod)
            {
                _velocity = _jumpSpeed;
                _jumpButtonPressedTime = null;
                _lastGroundedTime = null;
            }
        }
    }

}
