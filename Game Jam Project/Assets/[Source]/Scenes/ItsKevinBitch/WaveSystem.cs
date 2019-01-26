using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public List<BriefSpawner> spawners = new List<BriefSpawner>();

    void Start() 
    {
        foreach (BriefSpawner spawner in GameObject.FindObjectsOfType<BriefSpawner>())
        {
            spawners.Add(spawner);
        }
    }

    
}
