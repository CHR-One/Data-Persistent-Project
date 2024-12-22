using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class HighScores : MonoBehaviour
{
    public ScoreManager scoreManager;
    private GameObject fieldName, fieldScore;
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        scoreManager.LoadScores();

        string highscoresPath = Application.persistentDataPath + "/highscores.json";
        if (scoreManager.scoreList.Count > 0)
        {
            foreach (var item in scoreManager.scoreList)
            {
                int suffix = scoreManager.scoreList.IndexOf(item) + 1;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
    