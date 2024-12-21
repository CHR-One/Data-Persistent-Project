using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public int LineCount { get; private set; }
    public float BallSpeed { get; private set; }
    public float RangeDirection { get; private set; }

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

    public void Load()
    {
        this.LineCount = 6;
        this.BallSpeed = 2;
        this.RangeDirection = 1.0f;
    }

    public void Save()
    {
        
    }
}
