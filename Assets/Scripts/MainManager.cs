using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    private List<int> _unlockedLevels;

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
        public List<int> unlockedLevels = new List<int>();
    }

    public void SaveProgress(int level)
    {
        SaveData data = new SaveData();
        
        data.unlockedLevels.Add(level);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json",json);
    }

    public void LoadProgress()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            _unlockedLevels = data.unlockedLevels;
        }
    }
}
