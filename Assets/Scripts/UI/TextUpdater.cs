using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace UI
{
    [DefaultExecutionOrder(1002)]
    public class TextUpdater : MonoBehaviour
    {
        [FormerlySerializedAs("bounceLimit")] public TMP_Text bounceLimitText;
        public TMP_Text currentBounceAmount;
        public PlayerController playerController;
        private int bounceLimit = -1;
        private void Start()
        {
            playerController ??= GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            if (playerController == null)
            {
                playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            }
            // Subscribe to the BounceLimitChanged event.
            playerController.BounceLimitChanged += UpdateLimitText;
            playerController.CurrentBounceCountChanged += UpdateCurrentBounceAmount;
            UpdateLimitText(playerController.BounceLimit);
            UpdateCurrentBounceAmount(playerController.CurrentBounceCount);
        }

        private void OnDisable()
        {
            // Unsubscribe from the BounceLimitChanged event when the script is disabled.
            playerController.BounceLimitChanged -= UpdateLimitText;
            playerController.CurrentBounceCountChanged -= UpdateCurrentBounceAmount;
        }

        private void UpdateLimitText(int newValue)
        {
            bounceLimit = newValue;
            // Update the text component with the new bounce limit value.
            bounceLimitText.text = "Bounce limit: " + newValue;
            UpdateCurrentBounceAmount(playerController.CurrentBounceCount);
        }

        private void UpdateCurrentBounceAmount(int bouncesMade)
        {
            currentBounceAmount.text = "Bounces left: " + (bounceLimit == -1 ? "infinite" : bounceLimit - bouncesMade);
        }
    }
}