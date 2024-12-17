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

    // Start is called before the first frame update
    void Start()
    {
        playerInputField = GameObject.Find("Player Name Input");
    }

    public void StartGame()
    {
        string player = playerInputField.GetComponent<TMP_InputField>().text;
        Player.Instance.SetPlayerName(player);
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
