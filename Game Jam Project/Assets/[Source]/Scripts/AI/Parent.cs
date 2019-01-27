using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : NavAgent
{
    public float remainingtime, randomNumber, randomTime;
    public int min = 20, max = 60;
    public List<Transform> checkpoints = new List<Transform>();


    public static Parent Instance;
    public event Action<float> HappinessChanged;
    public float Happiness
    {
        get
        {
            return happiness;
        }
        set
        {
            happiness = value;
            if(happiness <= 0)
            {
                happiness = 0;
                GameManager.Instance.gameOver = true;
                Debug.LogError("GAME OVER!");
            }
            HappinessChanged(happiness);
        }
    }

    private float happiness;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        happiness = 100f;
    }
    void Update() 
    {
        Timer();
    }
    void Timer()
    {
        if(remainingtime <= 0f)
        {
            MoveToCheckpoint();
            randomNumber = UnityEngine.Random.Range(min, max);
            remainingtime = randomTime;
        }
        remainingtime -= Time.deltaTime;

    }
    void MoveToCheckpoint()
    {
        int i = UnityEngine.Random.Range(0, checkpoints.Count);
        Vector3 pos = checkpoints[i].position;
        MoveTo(pos);
    }
}
