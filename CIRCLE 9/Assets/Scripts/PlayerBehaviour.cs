using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _orientation;
    private CharacterController _controller;
    private float _speed = 10f;
    private Vector3 _movement;
    private Vector3 _movementDirection;
    private Vector3 _moveDirection;
    public bool _isMoving = false;

    private float _horizontalInput;
    private float _verticalInput;

    private float _mouseSensitivity = 2f;
    private float _cameraVerticalRotation = 0f;
    private float _gravity = -1f;
    private float _velocity;
    private float _jumpSpeed = 0.3f;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }

    void Update()
    {
        Camera();
        Jumping();
        Debug.Log(_camera.fieldOfView);

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        //_controller.SimpleMove(moveDirection * _speed);
    }
        if(Input.GetKey(KeyCode.LeftShift)&& _isMoving)
        {
            _speed = 20f;
            if(_camera.fieldOfView <=100)
            {
                _camera.fieldOfView += 40 * Time.deltaTime;
            }
        }
        else
        {
            if(_camera.fieldOfView >= 80)
            {
                _camera.fieldOfView -= 40 * Time.deltaTime;
            }
            _speed = 10f;
        }
    }


    private void FixedUpdate()
    {
        Movement();
        ApplyGravity();
        

        if (_moveDirection.sqrMagnitude > 0.5f) _isMoving = true;
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
        if(!_controller.isGrounded)
        {
            _velocity += _gravity * Time.fixedDeltaTime;
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
        if (Input.GetKeyDown(KeyCode.Space) && _controller.isGrounded)
        {
            _velocity = _jumpSpeed;
        }
    }
    
}
