using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _orientation;
    private CharacterController _controller;
    private float _speed = 10f;
    private Vector3 _movement;
    private Vector3 _movementDirection;
    private Vector3 moveDirection;
    private bool _isMoving = false;

    private float _horizontalInput;
    private float _verticalInput;

    private float _mouseSensitivity = 2f;
    private float _cameraVerticalRotation = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }

    void Update()
    {
        //Debug.Log(_isMoving);
        //Locomotion();
        Camera();

        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        Debug.Log(moveDirection);
        //_controller.SimpleMove(moveDirection * _speed);
    }


    private void FixedUpdate()
    {
        moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
        _controller.Move(moveDirection * _speed * Time.deltaTime);

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
    //private void MovePlayer()
    //{
    //    _movementDirection = _orientation.forward * _verticalInput + _orientation.right* _horizontalInput;
    //    _rb.AddForce(_movementDirection.normalized * _speed, ForceMode.Force);
    //}
    
}
