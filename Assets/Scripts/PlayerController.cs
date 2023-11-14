
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 _touchStartPos; // Store the initial touch position.
    private Rigidbody2D _rb; // Reference to the Rigidbody2D component of the player (ball).
    public float launchPower = 0.1f; // Adjust this to control the sensitivity of drag.
    private bool _canShot;
    public Vector2 startPosition = new(0, 0);
    private Vector2 _dragDistance = Vector2.one;
    public float maxSpeed = 11f;
    private Vector2 _activeCheckpoint = new Vector2(0,0);
    public Vector2 getDragDistance() => _dragDistance;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _canShot = true;
        _rb.gravityScale = 0f;
        _rb.simulated = true;
        _activeCheckpoint = startPosition;
        transform.position = startPosition;
    }

    void Update()
    {
        HandleInput();
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);
        BallStoppedMoving();
    }

    //todo: ball should be considered not moving even if it is moving REALLY slowly?
    private void BallStoppedMoving()
    {
        if (_canShot)
            return;

        if(_rb.velocity == Vector2.zero)
        {
            Reset();
        }
    }

    // Handle touch input for mobile devices.
    private void HandleInput()
    {
        if (!_canShot)
            return;

        if (Input.touchCount > 0)
        {
            // Handle touch input.
            Touch touch = Input.GetTouch(0); // Get the first touch.
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Capture the initial touch position.
                    _touchStartPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    _dragDistance = touch.position - _touchStartPos;
                    break;
                case TouchPhase.Ended:
                    // Release the ball, apply a launch force based on drag distance.
                    _rb.gravityScale = 1f;
                    LaunchBall(-_dragDistance);
                    _canShot = false;
                    break;
            }
        }
    }

    // Function to launch the ball based on drag distance.
    void LaunchBall(Vector2 dragDistance)
    {
        // Calculate the launch force based on the drag distance (you may need to adjust this).
        Vector2 launchDirection = dragDistance * launchPower;

        // Apply the launch force to the player Rigidbody2D.
        _rb.AddForce(launchDirection, ForceMode2D.Impulse);
    }

    public void Reset()
    {
        transform.position = _activeCheckpoint;
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
        _canShot = true;
    }

    public void UpdateCheckpoint(Vector2 newCheckPointPos)
    {
        _activeCheckpoint = newCheckPointPos;
    }
}