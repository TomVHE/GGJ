using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Utilities;

public class GameManager : SerializedSingleton<GameManager>
{
    public event Action<int> Killed;
    public event Action<int> Spawned;
    public int KilledLetters
    {
        get => killedLetters;
        set
        {
            killedLetters = value;
            Killed(killedLetters);
        }
    }
    public int LettersSpawned
    {
        get => lettersSpawned;
        set
        {
            lettersSpawned = value;
            Spawned(lettersSpawned);
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
