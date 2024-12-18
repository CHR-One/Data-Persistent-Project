using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    private Player player;

    public Text ScoreText;
    public Text bestScoreText;
    public int bestScoreValue = 0;
    public GameObject GameOverText;
    private List<SaveData> scoreList;
    private int maxScores = 8;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        InitGame();        
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    private void InitGame()
    {
        //Retrieve player name and show it in the score text
        player = GameObject.Find("Player").GetComponent<Player>();
        ScoreText.text = $"{player.playerName}'s score: 0";

        //Load highscore, if presente
        LoadBestScore();

        //Fill the board with bricks
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{player.playerName}'s score: {m_Points}";

        //Automatically change the best score text if the player points reach it
        if (m_Points > bestScoreValue)
        {
            int bestScoreValueTemp = m_Points;
            bestScoreText.text = $"Best score: {player.playerName} {bestScoreValueTemp}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (m_Points >= bestScoreValue)
        {
            SaveScore(player.playerName, m_Points);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }


    [System.Serializable]
    class SaveData
    {
        public string name;
        public int score;
    }

    private void SaveScore(string player, int score)
    {
        SaveData playerData = new SaveData();
        playerData.name = player;
        playerData.score = score;

        //add the new score to the list
        scoreList.Add(playerData);
        string json = JsonUtility.ToJson(scoreList);
        File.WriteAllText(Application.persistentDataPath + "/highscores.json", json);
    }

    private void LoadBestScore()
    {
        //Retrieve highscore.json
        string highscoresPath = Application.persistentDataPath + "/highscores.json";

        // Check if hiscores.json exists. If it exists, load the best score
        if (File.Exists(highscoresPath))
        {
            string json = File.ReadAllText(highscoresPath);
            List<SaveData> highscoresData = JsonUtility.FromJson<List<SaveData>>(json);

            //Retrieve the best score
            SaveData bestPlayer = highscoresData[0];
            bestScoreValue = bestPlayer.score;
            bestScoreText.text = $"Best score : {bestPlayer.name} {bestScoreValue}";
        }
        else
        {
            bestScoreText.text = $"Best score: {bestScoreValue}";
        }
    }
}
