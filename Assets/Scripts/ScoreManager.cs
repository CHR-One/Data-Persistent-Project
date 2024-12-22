using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Singleton
    public static ScoreManager Instance;
    public List<SaveData> scoreList;

    // SaveData class to store in highscore.json
    [System.Serializable]
    public class SaveData
    {
        public string Name;
        public int Score;

        public SaveData(string playerName, int playerScore)
        {
            this.Name = playerName;
            this.Score = playerScore;
        }
    }

    [System.Serializable]
    public class SaveDataList
    {
        public List<SaveData> Highscore;
        public int maxScores = 8;

        public SaveDataList()
        {
            this.Highscore = new List<SaveData>(maxScores);
        }
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        scoreList = new List<SaveData>();
        LoadScores();
        DontDestroyOnLoad(gameObject);
    }

    // Load the highscore list from highscore.json
    public void LoadScores()
    {
        string path = Application.persistentDataPath + "/highscores.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataList saveDataList = JsonUtility.FromJson<SaveDataList>(json);
            scoreList = saveDataList.Highscore;
        }
        else
        {
            Debug.Log("No highscores found");
        }
    }

    // Save the highscore list to highscore.json
    public void SaveScore(string player, int score)
    {
        string path = Application.persistentDataPath + "/highscores.json";
        SaveDataList saveDataList = new()
        {
            Highscore = new List<SaveData>()
        };

        // Add the new score to the list
        saveDataList.Highscore = scoreList;
        saveDataList.Highscore.Add(new SaveData(player, score));

        // Sort the list by score
        saveDataList.Highscore = saveDataList.Highscore.OrderByDescending(x => x.Score).ToList();

        // If the list is bigger than maxScores, remove the last element
        if (saveDataList.Highscore.Count > saveDataList.maxScores)
        {
            saveDataList.Highscore.RemoveAt(saveDataList.maxScores);
        }

        // Save the new list to highscore.json
        string json = JsonUtility.ToJson(saveDataList);
        File.WriteAllText(path, json);
    }
}
