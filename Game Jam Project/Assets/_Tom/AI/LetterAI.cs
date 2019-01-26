using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterAI : NavAgent
{
    [SerializeField] private Transform target;

    private void Update()
    {
        MoveTo(target.position);
    }
}
