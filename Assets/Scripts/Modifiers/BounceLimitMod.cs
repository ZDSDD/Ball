using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceLimitMod : MonoBehaviour
{
    public int newBounceLimit; // The new bounce limit to set for the player
    public bool onlyAdd = false; // If true, only add to the current bounce limit
    public int addToLimit = 0; // Amount to add to the current bounce limit

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController)
        {
            if (!onlyAdd)
            {
                playerController.BounceLimit = newBounceLimit;
            }
            
            if (playerController.BounceLimit == -1)
            {
                return;
            }
            
            if (playerController.BounceLimit + addToLimit <= -1)
            {
                playerController.BounceLimit = 0;
                return;
            }
            
            playerController.BounceLimit += addToLimit;
        }
    }
}