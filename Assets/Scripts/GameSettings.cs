using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public string path;

    // SaveData class to store in path value
    [System.Serializable]
    public class SettingsData
    {
        public int LineCount;
        public float BallSpeed;
        public float RangeDirection;

        public SettingsData(int lineCount, float ballSpeed, float rangeDirection)
        {
            this.LineCount = lineCount;
            this.BallSpeed = ballSpeed;
            this.RangeDirection = rangeDirection;
        }
    }

    public SettingsData settings;

    //Singleton pattern
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        path = Application.persistentDataPath + "/settings.json";
        Load();
        DontDestroyOnLoad(gameObject);
    }
    
    public void Load()
    {
        settings = new SettingsData(0, 0, 0);
        // Retrieve settings from file
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            settings = JsonUtility.FromJson<SettingsData>(json);            
        }
        else
        {
            SetDefaults();

            //Create a default gamesettings.json
            string json = JsonUtility.ToJson(settings);
            File.WriteAllText(path, json);
        }
    }

    public void SetDefaults()
    {
        //LineCount = 6;
        settings.LineCount = 6;
        //BallSpeed = 2;
        settings.BallSpeed = 2.0f;
        //RangeDirection = 1.0f;
        settings.RangeDirection = 1.0f;
    }
}
