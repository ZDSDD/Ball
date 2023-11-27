using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

[DefaultExecutionOrder(1001)]
public class LevelUIManager : MonoBehaviour
{
    public Button resetButton;
    public Button menuButton;
    public PlayerController playerController;
    [FormerlySerializedAs("pausePanel")] public GameObject levelFinishedPanel;

    private void Start()
    {
        playerController ??= GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        playerController.onLevelComplete += OnLevelComplete;
        resetButton.onClick.AddListener(ResetLevel);
        resetButton.gameObject.SetActive(true);
        levelFinishedPanel.SetActive(false);
        menuButton.onClick.AddListener(GoToMenu);
    }

    private void ResetLevel()
    {
        resetButton.gameObject.SetActive(false);
        playerController.Reset();
        StartCoroutine(ResetButtonCooldown());
    }

    private IEnumerator ResetButtonCooldown()
    {
        yield return new WaitForSeconds(3f);
        resetButton.gameObject.SetActive(true);
    }

    private void OnLevelComplete()
    {
        resetButton.gameObject.SetActive(false);
        levelFinishedPanel.SetActive(true);
    }
    private void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}