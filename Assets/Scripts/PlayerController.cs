
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 _touchStartPos; // Store the initial touch position.
    private Rigidbody2D _rb; // Reference to the Rigidbody2D component of the player (ball).
    public float launchPower = 0.1f; // Adjust this to control the sensitivity of drag.
    private bool _canShot;
    public Vector2 startPosition = new(0, -3.9f);
    private Vector2 _dragDistance = Vector2.one;
    public Vector2 getDragDistance() => _dragDistance;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _canShot = true;
        _rb.gravityScale = 0f;
        _rb.simulated = true;
        transform.position = startPosition;
    }

    void Update()
    {
        HandleInput();
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
        transform.position = startPosition;
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
        _canShot = true;
    }
}