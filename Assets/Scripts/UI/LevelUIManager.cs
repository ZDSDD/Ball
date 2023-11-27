using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    public Button resetButton;
    public PlayerController playerController;

    private void Start()
    {
        if (playerController is null)
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.onLevelComplete += OnLevelComplete;
        }

        if (resetButton)
        {
            resetButton.onClick.AddListener(ResetLevel);
            resetButton.gameObject.SetActive(true);
        }
    }

    private void ResetLevel()
    {
        resetButton.gameObject.SetActive(false);
        playerController.Reset();
        StartCoroutine(ResetButtonCooldown());
    }

    private IEnumerator ResetButtonCooldown()
    {
        yield return new WaitForSeconds(3);
        resetButton.gameObject.SetActive(true);
    }

    private void OnLevelComplete()
    {
        resetButton.gameObject.SetActive(false);
    }
}