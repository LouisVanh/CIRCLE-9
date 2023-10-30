using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _orientation;
    //private CharacterController _controller;
    private float _speed = 2f;
    private Vector3 _movement;
    private Vector3 _movementDirection;
    private bool _isMoving = false;

    private float _horizontalInput;
    private float _verticalInput;

    private float _mouseSensitivity = 2f;
    private float _cameraVerticalRotation = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //_controller= GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }

    void Update()
    {
        Debug.Log(_rb.velocity.magnitude);
        //Debug.Log(_isMoving);
        //Locomotion();
        float xInput = Input.GetAxis( "Mouse X" ) * _mouseSensitivity;
        float yInput = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _cameraVerticalRotation -= yInput;
        _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -90f, 90f);
        _camera.transform.localEulerAngles = Vector3.right * _cameraVerticalRotation;
        transform.Rotate(Vector3.up * xInput);

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        //Vector3 moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
        //moveDirection.Normalize();

        //_controller.SimpleMove(moveDirection * _speed);
    }
    private void FixedUpdate()
    {
        MovePlayer();
    } 
    private void MovePlayer()
    {
        _movementDirection = _orientation.forward * _verticalInput + _orientation.right* _horizontalInput;
        _rb.AddForce(_movementDirection.normalized * _speed, ForceMode.Impulse);
    }
    
}
