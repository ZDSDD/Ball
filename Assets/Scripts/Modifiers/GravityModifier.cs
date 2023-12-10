using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

public class GravityModifier : MonoBehaviour
{
    public enum GravityDirection
    {
        Down,
        Left,
        Up,
        Right,
        Undefined
    };

    private Dictionary<GravityDirection, Vector2> _gravityVectorsMap;
    public GravityDirection mGravityDirection;
    private GravityDirection _currentGravityDirection;
    public bool justReverse;

    private void Start()
    {
        _gravityVectorsMap = new Dictionary<GravityDirection, Vector2>
        {
            { GravityDirection.Down, new Vector2(0, -9.8f) },
            { GravityDirection.Up, new Vector2(0, 9.8f) },
            { GravityDirection.Left, new Vector2(-9.8f, 0) },
            { GravityDirection.Right, new Vector2(9.8f, 0) },
            { GravityDirection.Undefined, new Vector2(0f, 0) }
        };
        Physics2D.gravity = _gravityVectorsMap[GravityDirection.Down];
        _currentGravityDirection = GravityDirection.Down;
    }

    public GravityDirection GetCurrentGravityDirection()
    {
        return _currentGravityDirection;
    }

    /*
     * Changes gravity direction on trigger.
     */
    private GravityDirection GetReversedGravityDirection()
    {
        switch (GetCurrentGravityDirection())
        {
            case GravityDirection.Down: return GravityDirection.Up;
            case GravityDirection.Up: return GravityDirection.Down;
            case GravityDirection.Right: return GravityDirection.Left;
            case GravityDirection.Left: return GravityDirection.Right;
            default: return GravityDirection.Undefined;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _currentGravityDirection =  UpdateGravityDirection();
        if (!other.CompareTag("Player")) return;

        
        if (justReverse)
        {
            _currentGravityDirection = GetReversedGravityDirection();
        }
        else
        {
            _currentGravityDirection = mGravityDirection;
        }
        Physics2D.gravity = _gravityVectorsMap[_currentGravityDirection];
    }

    private GravityDirection UpdateGravityDirection()
    {
        foreach(var entry in _gravityVectorsMap)
        {
            if(entry.Value == Physics2D.gravity)
            {
                return entry.Key;
            }
        }
        return GravityDirection.Undefined;
    }
}