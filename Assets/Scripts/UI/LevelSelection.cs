using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] levelButtons;
    public Button resetButton;

    private void Awake()
    {
        MainManager.Instance.LoadProgress();
    }

    void Start()
    {
        HashSet<int> unlockedLevels = MainManager.Instance.UnlockedLevels;
        // Assign the button click event handlers
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1; // Assuming your levels are named "Level1", "Level2", etc.
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
            levelButtons[i].interactable = false;
        }

        foreach (int unlockIndex in unlockedLevels)
        {
            //check in case unlockIndex is out of bounds
            if (levelButtons.Length > unlockIndex)
                levelButtons[unlockIndex].interactable = true;
        }


        resetButton.interactable = true;
        resetButton.onClick.AddListener(() =>
        {
            MainManager.Instance.ResetProgress();
            Debug.Log("resetButton.onClick");
            LoadLevel(SceneManager.GetActiveScene().name);
        });
    }

    void LoadLevel(int levelIndex)
    {
        string levelName = "Level" + levelIndex; // Adjust the naming convention if needed
        SceneManager.LoadScene(levelName);
    }

    void LoadLevel(String levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}