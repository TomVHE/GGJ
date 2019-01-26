using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Utilities;

public class GameManager : SerializedSingleton<GameManager>
{
    public event Action<int> ScoreChanged;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            ScoreChanged(score);
        }
    }

    private int score;
}
