﻿using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance { get { return _instance; } }


    private PlayerControls _playerControls;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        _playerControls = new PlayerControls();
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    /// <summary>
    /// Get player movement input
    /// </summary>
    public Vector2 GetPlayerMovement()
    {
        return _playerControls.Player.Movement.ReadValue<Vector2>();
    }

    /// <summary>
    /// Get delta from Mouse when player is looking around
    /// </summary>
    public Vector2 GetMouseDelta()
    {
        return _playerControls.Player.Look.ReadValue<Vector2>();
    }

    /// <summary>
    /// Checks if the Player has pressed the Jump button
    /// </summary>
    public bool PlayerJump()
    {
        return _playerControls.Player.Jump.triggered;
    }
}
