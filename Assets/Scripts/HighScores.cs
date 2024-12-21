using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class HighScores : MonoBehaviour
{
    public MainManager manager;
    public List<MainManager.SaveData> scoreList;
    private GameObject fieldName, fieldScore;
    // Start is called before the first frame update
    void Start()
    {
        string highscoresPath = Application.persistentDataPath + "/highscores.json";
        if (File.Exists(highscoresPath))
        {
            string json = File.ReadAllText(highscoresPath);
            MainManager.SaveDataList saveDataList = JsonUtility.FromJson<MainManager.SaveDataList>(json);
            scoreList = saveDataList.Highscore;

            foreach (var item in scoreList)
            {
                int suffix = scoreList.IndexOf(item) + 1;
                fieldName = GameObject.Find("Name" + suffix);
                fieldScore = GameObject.Find("Score" + suffix);
                fieldName.GetComponent<TextMeshProUGUI>().text = item.Name;
                fieldScore.GetComponent<TextMeshProUGUI>().text = item.Score.ToString();
            }
        }
        else
        {
            Debug.Log("No highscores found");
        }
    }
}
