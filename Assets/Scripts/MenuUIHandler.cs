using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        Player.Instance.SetPlayerName();
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
}
