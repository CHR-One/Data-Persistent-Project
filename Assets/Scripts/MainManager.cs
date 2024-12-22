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
    public Rigidbody Ball;
    public Player player;
    public Text ScoreText;
    public Text bestScoreText;
    public int bestScoreValue = 0;
    public GameObject GameOverText;
    private GameSettings GameSettings;
    private ScoreManager scoreManager;
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
        //Retrieve persistent objects
        player = GameObject.Find("Player").GetComponent<Player>();
        GameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        //Retrieve the game settings
        GameSettings.Load();

        //Show the best score and the player's score        
        ScoreText.text = $"{player.playerName}'s score: 0";

        scoreManager.LoadScores();
        

        if (scoreManager.scoreList.Count > 0)
        {
            bestScoreValue = scoreManager.scoreList.ElementAt(0).Score;
            bestScoreText.text = $"Best score: {scoreManager.scoreList.ElementAt(0).Name} {bestScoreValue}";
        }
        else
        {
            bestScoreText.text = "Best score: 0";
        }

        //Fill the board with bricks
        FillBoard();
    }

    private void StartGame()
    {
        m_Started = true;
        float randomDirection = Random.Range(-GameSettings.RangeDirection, GameSettings.RangeDirection);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * GameSettings.BallSpeed, ForceMode.VelocityChange);
    }

    private void FillBoard()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < GameSettings.LineCount; ++i)
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
        scoreManager.SaveScore(player.playerName, m_Points);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
