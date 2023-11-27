using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CooldownManager : MonoBehaviour
{
    public float cooldownDuration { set; get; } // Duration of the cooldown in seconds
    private float cooldownTimer; // Timer for tracking the remaining cooldown time
    private bool isCooldownActive; // Flag to check if the cooldown is currently active
    public Action onCooldownComplete;
    public Action<float> onValueChange;


    void Start()
    {
        // Initialize the cooldown timer
        cooldownTimer = cooldownDuration;
        isCooldownActive = false;
    }


    void Update()
    {
        // Check if the cooldown is active
        if (isCooldownActive)
        {
            // Update the cooldown timer
            cooldownTimer -= Time.deltaTime;
            onValueChange.Invoke(cooldownTimer / cooldownDuration);
            // Check if the cooldown has reached zero
            if (cooldownTimer <= 0f)
            {
                // Cooldown has ended
                cooldownTimer = 0f;
                isCooldownActive = false;
                onCooldownComplete.Invoke();
            }
        }
    }

    // Function to start the cooldown
    public void StartCooldown()
    {
        if (!isCooldownActive)
        {
            // Start the cooldown
            isCooldownActive = true;
            cooldownTimer = cooldownDuration;
        }
    }
}