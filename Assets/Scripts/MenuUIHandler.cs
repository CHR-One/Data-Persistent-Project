using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    public static string playerName;
    public InputField playerInputField;

    // Start is called before the first frame update
    void Start()
    {
        playerInputField = GetComponent<InputField>();
    }

    public void StartGame()
    {
        SetPlayerName();
        SceneManager.LoadScene(1);
    }

    public void QuitApp()
    {

    }
    
    public void ChangeSettings()
    {

    }

    public void ShowScores()
    {

    }

    public void SetPlayerName()
    {
        playerName = playerInputField.text;
        Debug.Log($"{playerName}");
    }
}
