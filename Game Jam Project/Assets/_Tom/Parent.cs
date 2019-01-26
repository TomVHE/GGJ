using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour
{
    public static Parent instance;
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

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
