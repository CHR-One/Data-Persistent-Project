using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public string playerName;
    public TMP_InputField playerInputField;

    private void Awake()
    {
        if (Instance != null)
        {        
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerName()
    {
        playerName = playerInputField.text;
    }
}
