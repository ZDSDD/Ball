using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float launchPower = 0.1f; // Adjust this to control the sensitivity of drag.
    public float maxSpeed = 11f;
    public Vector2 startPosition = new(0, 0);
    public ProjectionDisplay _projectiondisplayRef;

    private Vector2 _touchStartPos; // Store the initial touch position.
    private Checkpoint _activeCheckpoint;
    private Vector2 _dragDistance = Vector2.one;
    public Vector2 getDragDistance() => _dragDistance;

    private Rigidbody2D _rb; // Reference to the Rigidbody2D component of the player (ball).

    public Rigidbody2D Rb => _rb;

    // Define the event using Action with a generic parameter.
    public event Action<int> BounceLimitChanged;

    // The actual variable
    private int _bounceLimit = -1;

    public int BounceLimit
    {
        get => _bounceLimit;
        set
        {
            if (_bounceLimit == value) return;
            
            _bounceLimit = value;
            // Invoke the event when the value changes.
            OnBounceLimitChanged();
        }
    }

    protected virtual void OnBounceLimitChanged()
    {
        // Fire the event.
        BounceLimitChanged?.Invoke(BounceLimit);
    }

    public event Action<int> CurrentBounceCountChanged;

    private int _currentBounceCount;

    public int CurrentBounceCount
    {
        get => _currentBounceCount;
        private set
        {
            if (value == _currentBounceCount) return;
            _currentBounceCount = value;
            OnCurrentBounceCountChanged();
    }
    }
    
    protected virtual void OnCurrentBounceCountChanged()
    {
        CurrentBounceCountChanged?.Invoke(CurrentBounceCount);
    }

    private bool _canShot;
    private bool _levelComplete;

    public Action onLaunchComplete;
    public Action onResetEnter;

    public CooldownManager CooldownAfterReset { private set; get; }

    public bool LevelComplete
    {
        get => _levelComplete;
        set => _levelComplete = value;
    }

    void Start()
    {
        //setup 
        CooldownAfterReset = gameObject.AddComponent<CooldownManager>();
        CooldownAfterReset.cooldownDuration = 2f;
        CooldownAfterReset.onCooldownComplete += () => _canShot = true;

        _rb = GetComponent<Rigidbody2D>();
        _canShot = true;
        _rb.gravityScale = 0f;
        _rb.simulated = true;
        _activeCheckpoint = GameObject.FindGameObjectWithTag("Start").GetComponent<Checkpoint>();
        if (_activeCheckpoint)
        {
            startPosition = _activeCheckpoint.transform.position;
            if (_activeCheckpoint.resetBounceLimit)
            {
                BounceLimit = _activeCheckpoint.newBounceLimit;
            }
        }

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
        if (_levelComplete || _canShot)
            return;

        if (_rb.velocity == Vector2.zero)
        {
            Reset();
        }
    }

    // Handle touch input for mobile devices.
    private void HandleInput()
    {
        if (!_canShot || _levelComplete)
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
                    _projectiondisplayRef.SimulateTrajectory(this, -_dragDistance);
                    break;
                case TouchPhase.Ended:
                    // Release the ball, apply a launch force based on drag distance.
                    _rb.gravityScale = 1f;
                    _projectiondisplayRef.ResetDisplay();
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

        onLaunchComplete.Invoke();
    }

    public void Reset()
    {
        if (onResetEnter != null)
        {
            onResetEnter.Invoke();
        }
        else
        {
            //This was done for display simulation
            return;
        }

        transform.position = _activeCheckpoint.transform.position;
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
        CurrentBounceCount = 0;
        if (_activeCheckpoint.resetBounceLimit)
        {
            BounceLimit = _activeCheckpoint.newBounceLimit;
        }

        CooldownAfterReset.StartCooldown();
    }


    public void UpdateCheckpoint(Checkpoint checkpoint)
    {
        _activeCheckpoint = checkpoint;
        
        if (checkpoint.newBounceLimit >= -1)
        {
            this.BounceLimit = checkpoint.newBounceLimit;
        }
        if (checkpoint.resetBounceLimit)
        {
            this.BounceLimit = BounceLimit;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Handle bounce limit mechanic
        // '-1' means unlimited bounces are allowed
        if (BounceLimit > -1)
        {
            if (CurrentBounceCount >= BounceLimit)
            {
                Reset();
            }
            else
            {
                CurrentBounceCount++;
                Debug.Log("CurrentBounceCount: " + CurrentBounceCount + ", BounceLimit: " + BounceLimit);
            }
        }
        //Handle wrong bounce limit 
        else if (BounceLimit < -1)
        {
            BounceLimit = -1;
        }
    }
    
}