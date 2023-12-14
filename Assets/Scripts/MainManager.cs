using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    
    private HashSet<int> _unlockedLevels;

    private int _currentlyPlayedLevel;
    
    public Action onLevelComplete;

    public HashSet<int> UnlockedLevels => _unlockedLevels;

    // MainManager is a singleton
    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        onLevelComplete += OnLevelComplete;
        LoadProgress();
        DontDestroyOnLoad(gameObject);
    }
    [Serializable]
    class SaveData
    {
        public List<int> unlockedLevels;
    }

    public void SaveProgress(int level)
    {
        SaveData data = new SaveData();
        _unlockedLevels.Add(level);
        data.unlockedLevels = new List<int>(_unlockedLevels);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json",json);
    }

    public void LoadProgress()
    {
        _unlockedLevels = new HashSet<int>(){0};
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            _unlockedLevels.AddRange(data.unlockedLevels);
        }
    }

    private void OnLevelComplete()
    {
        String levelName = SceneManager.GetActiveScene().name;
        // Extract the numeric part from the string
        string numericPart = levelName.Substring("Level".Length);

        // Parse the numeric part to an integer
        if (int.TryParse(numericPart, out int levelIndex))
        {
            // Successfully parsed, levelIndex now contains the numeric value
            SaveProgress(levelIndex);
        }
        else
        {
            // Parsing failed, handle the error
            throw new Exception("Invalid level name format");
        }
    }

    public void ResetProgress()
    {
        SaveData data = new SaveData();
        _unlockedLevels = new HashSet<int>() { 0 };
        data.unlockedLevels = new List<int>(_unlockedLevels);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json",json);
        LoadProgress();
    }

}
