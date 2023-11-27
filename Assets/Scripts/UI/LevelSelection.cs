using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] levelButtons;

    void Start()
    {
        // Assign the button click event handlers
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1; // Assuming your levels are named "Level1", "Level2", etc.
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    void LoadLevel(int levelIndex)
    {
        string levelName = "Level" + levelIndex; // Adjust the naming convention if needed
        SceneManager.LoadScene(levelName);
    }
}
