﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Utilities;

public class GameManager : SerializedSingleton<GameManager>
{
    public event Action<int> Killed;
    public event Action<int> Spawned;

    public bool gameOver;

    public int KilledLetters
    {
        get => killedLetters;
        set
        {
            killedLetters = value;
            //Debug.Log("K" + killedLetters);
            //Killed(killedLetters);
        }
    }
    public int LettersSpawned
    {
        get => lettersSpawned;
        set
        {
            lettersSpawned = value;
            //Debug.Log("S" + lettersSpawned);
            //Spawned(lettersSpawned);
        }
    }
    public int CurrentWave
    {
        get => currentWave;
        set
        {
            currentWave = value;
            Debug.Log("Wave changed! Current wave: " + currentWave);
        }
    }
    private int killedLetters;
    private int lettersSpawned;
    private int currentWave = 1;
}
