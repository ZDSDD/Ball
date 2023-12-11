using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    
    private HashSet<int> _unlockedLevels;

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

}
