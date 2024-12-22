using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    private GameObject playerInputField;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        playerInputField = GameObject.Find("Player Name Input");
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void StartGame()
    {
        string playerName = playerInputField.GetComponent<TMP_InputField>().text;
        if (playerName == "" || playerName == null)
        {
            player.SetPlayerName("Unknown");
        }
        else
        {
            player.SetPlayerName(playerName);
        }
        
        SceneManager.LoadScene(1);   
    }

    public void QuitApp()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    
    public void ChangeSettings()
    {
        SceneManager.LoadScene(3);
    }

    public void ShowScores()
    {
        SceneManager.LoadScene(2);
    }
}
