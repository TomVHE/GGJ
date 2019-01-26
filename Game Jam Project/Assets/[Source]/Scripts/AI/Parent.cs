using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : NavAgent
{
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
}
