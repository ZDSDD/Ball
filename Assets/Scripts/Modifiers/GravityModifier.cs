using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GravityModifier : MonoBehaviour
{
    public enum GravityDirection { Down, Left, Up, Right };
    public GravityDirection mGravityDirection;
    
    /*
     * Changes gravity direction on trigger.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        switch (mGravityDirection)
        {
            case GravityDirection.Down:
                Physics2D.gravity = new Vector2(0, -9.8f);
                break;

            case GravityDirection.Left:
                Physics2D.gravity = new Vector2(-9.8f, 0);
                break;

            case GravityDirection.Up:
                Physics2D.gravity = new Vector2(0, 9.8f);
                break;

            case GravityDirection.Right:
                Physics2D.gravity = new Vector2(9.8f, 0);
                break;
        }
    }
}
