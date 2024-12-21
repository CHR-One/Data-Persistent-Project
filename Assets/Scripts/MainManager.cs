using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Linq;

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
    private GameSettings GameSettings;

    public List<SaveData> scoreList;
    private int maxScores = 8;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;


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
        private int maxScores = 8;
        
        public SaveDataList()
        {
            this.Highscore = new List<SaveData>(maxScores);        
        }
    }

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
                StartGame();
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
        GameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
        ScoreText.text = $"{player.playerName}'s score: 0";
        //Retrieve the game settings
        GameSettings.Load();

        //Load highscore, if present
        LoadBestScore();

        //Fill the board with bricks
        FillBoard();
    }

    private void StartGame()
    {
        m_Started = true;
        float randomDirection = Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    private void FillBoard()
    {
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
            bestScoreText.text = $"Best score: {player.playerName} {m_Points}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveScore(player.playerName, m_Points);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void SaveScore(string player, int score)
    {        
        SaveDataList newScoreList = new();
        newScoreList.Highscore = new List<SaveData>();
        newScoreList.Highscore = scoreList;
        newScoreList.Highscore.Add(new SaveData(player, score));

        //Sort the list by score
        newScoreList.Highscore = newScoreList.Highscore.OrderByDescending(x => x.Score).ToList();

        //If the list is bigger than maxScores, remove the last element
        if (newScoreList.Highscore.Count > maxScores)
        {
            newScoreList.Highscore.RemoveAt(maxScores);
        }

        string json = JsonUtility.ToJson(newScoreList);
        File.WriteAllText(Application.persistentDataPath + "/highscores.json", json);
    }

    public void LoadBestScore()
    {
        //Initialize the score list
        scoreList = new List<SaveData>();

        //Retrieve highscore.json
        string highscoresPath = Application.persistentDataPath + "/highscores.json";

        // Check if hiscores.json exists. If it exists, load the best score
        if (File.Exists(highscoresPath))
        {
            string json = File.ReadAllText(highscoresPath);
            SaveDataList tempList = JsonUtility.FromJson<SaveDataList>(json);
            scoreList = tempList.Highscore;

            //Retrieve the best score
            if (scoreList.Count > 0)
            {                
                SaveData bestPlayer = scoreList.ElementAt(0);
                bestScoreValue = bestPlayer.Score;
                bestScoreText.text = $"Best score : {bestPlayer.Name} {bestScoreValue}";
            }
            else
            {
                bestScoreText.text = $"Best score: {bestScoreValue}";
            }
        }
        else
        {
            bestScoreText.text = $"Best score: {bestScoreValue}";
        }
    }
}
