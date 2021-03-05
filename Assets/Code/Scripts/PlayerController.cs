using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// https://youtu.be/5n_hmqHdijM?t=644

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("Movement")]
    private float _playerSpeed = 2.0f;
    [SerializeField]
    private float _jumpHeight = 1.0f;
    [SerializeField]
    private float _gravityValue = -9.81f;

    private Transform _cameraMainTransform;

    [SerializeField, Range(0.1f, 10f), Tooltip("Camera")]
    private float _lookSpeed = 4.0f;
    [SerializeField, Tooltip("Minimum angle, Use degrees, ex: 45, 90 etc.")]
    private float _minLookAngle = -90.0f;
    [SerializeField, Tooltip("Maximum angle, Use degrees, ex: 45, 90 etc.")]
    private float _maxLookAngle = 90.0f;

    private float _rotationX;

    private InputManager _inputManager;
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraMainTransform = Camera.main.transform;
        _inputManager = InputManager.Instance;
    }

    void Update()
    {
        Movement();
        Look();
    }

    /// <summary>
    /// Player Movement
    /// </summary>
    void Movement()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector2 movement = _inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = _cameraMainTransform.forward * move.z + _cameraMainTransform.right * move.x;
        move.y = 0;
        _controller.Move(move * Time.deltaTime * _playerSpeed);

        /*
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        */
        // Makes the Player "Jump"
        if (_inputManager.PlayerJump() && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    /// <summary>
    /// Player look around
    /// </summary>
    void Look()
    {
        float y = _inputManager.GetMouseDelta().x * _lookSpeed;
        _rotationX += _inputManager.GetMouseDelta().y * _lookSpeed;

        _rotationX = Mathf.Clamp(_rotationX, _minLookAngle, _maxLookAngle);

        transform.eulerAngles = new Vector3(-_rotationX, transform.eulerAngles.y + y, 0);
    }
}
