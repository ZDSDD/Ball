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
    public Button levelsButton;
    public Button menuButtonFinish;
    public Slider resetSliderCooldown;
    public PlayerController playerController;
    [FormerlySerializedAs("pausePanel")] public GameObject levelFinishedPanel;
    

    private void Start()
    {
        playerController ??= GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        

        levelsButton.onClick.AddListener(GoToLevelsMenu);
        
        menuButtonFinish.onClick.AddListener(GoToMenu);
        menuButton.onClick.AddListener(GoToMenu);

        resetButton.onClick.AddListener(ResetLevel);
        
        MainManager.Instance.onLevelComplete += OnLevelComplete;
        
        playerController.onLaunchComplete += () => resetButton.gameObject.SetActive(true);
        playerController.onResetEnter += () => resetButton.gameObject.SetActive(false);
        playerController.onResetEnter += OnResetStart;
        playerController.CooldownAfterReset.onValueChange += OnCooldownValueChange;
        playerController.CooldownAfterReset.onCooldownComplete += OnCooldownComplete;

        resetSliderCooldown.gameObject.SetActive(false);
        resetSliderCooldown.value = 0f;
        resetButton.gameObject.SetActive(false);
        levelFinishedPanel.SetActive(false);
        menuButton.gameObject.SetActive(true);
    }
    

    private void GoToLevelsMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void ResetLevel()
    {
        resetButton.gameObject.SetActive(false);
        playerController.Reset();
    }

    private void OnLevelComplete()
    {
        resetButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        levelFinishedPanel.SetActive(true);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnResetStart()
    {
        resetSliderCooldown.gameObject.SetActive(true);
    }

    private void OnCooldownValueChange(float x)
    {
        resetSliderCooldown.value = x;
    }

    private void OnCooldownComplete()
    {
        resetSliderCooldown.value = 0;
        resetSliderCooldown.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MainManager.Instance.onLevelComplete -= OnLevelComplete;
    }
}