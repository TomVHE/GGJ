using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public event Action<int> ScoreChanged;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            ScoreChanged(score);
        }
    }

    private int score;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
