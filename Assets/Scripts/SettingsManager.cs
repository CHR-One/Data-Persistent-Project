using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class SettingsManager : MonoBehaviour
{    
    public GameSettings.SettingsData settingsValues;
    private TMP_InputField linesInput;
    private TMP_InputField ballSpeedInput;
    private TMP_InputField rangeDirectionInput;
    public GameObject ErrorLineValue;
    public GameObject ErrorBallSpeedValue;
    public GameObject ErrorRangeDirectionValue;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the game settings
        InitSettings();        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void InitSettings()
    {
        //Retrieve the game settings       
        settingsValues = GameSettings.Instance.settings;

        //Fill the input fields with the current values
        linesInput = GameObject.Find("Lines Input").GetComponent<TMP_InputField>();
        ballSpeedInput = GameObject.Find("BallSpeed Input").GetComponent<TMP_InputField>();
        rangeDirectionInput = GameObject.Find("RangeDirection Input").GetComponent<TMP_InputField>();
        FillInput();
    }

    public void Save()
    {
        //Check if the values are valid. If not, show an error message and reset them to the previous values
        bool validLines = int.TryParse(linesInput.text, out int lines);
        bool validSpeed = float.TryParse(ballSpeedInput.text, out float speed);
        bool validDirection = float.TryParse(rangeDirectionInput.text, out float range);
        Debug.Log("Lines: " + lines + " Speed: " + speed + " Direction: " + range);
        Debug.Log("Valid Lines: " + validLines + " Valid Speed: " + validSpeed + " Valid Direction: " + validDirection);

        if (!validLines)
        {
            ErrorLineValue.SetActive(true);
        }
        else
        {
            ErrorLineValue.SetActive(false);
            settingsValues.LineCount = lines;
        }

        if (!validSpeed)
        {
            ErrorBallSpeedValue.SetActive(true);
        }
        else
        {
            ErrorBallSpeedValue.SetActive(false);
            settingsValues.BallSpeed = speed;
        }

        if (!validDirection)
        {
            ErrorRangeDirectionValue.SetActive(true);
        }
        else
        {
            ErrorRangeDirectionValue.SetActive(false);
            settingsValues.RangeDirection = range;
        }

        if (validLines && validSpeed && validDirection)
        {            
            //Save the settings
            string json = JsonUtility.ToJson(settingsValues);
            File.WriteAllText(GameSettings.Instance.path, json);
        }        
    }

    public void ResetSettings()
    {
        //Wrapper for GameSettings.SetDefaults
        GameSettings.Instance.SetDefaults();
        settingsValues = GameSettings.Instance.settings;
        FillInput();
    }

    private void FillInput()
    {
        linesInput.text = settingsValues.LineCount.ToString();
        ballSpeedInput.text = settingsValues.BallSpeed.ToString();
        rangeDirectionInput.text = settingsValues.RangeDirection.ToString();
    }
}
