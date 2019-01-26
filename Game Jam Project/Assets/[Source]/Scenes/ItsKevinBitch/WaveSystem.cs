using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public static WaveSystem instance;

    public List<BriefSpawner> spawners = new List<BriefSpawner>();

    int baseBrief;
    int briefenToSpawn;

    int basePlane;
    int planesToSpawn;

    int amountToSpawn;

    public static int aliveEnemies;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start() 
    {
        foreach (BriefSpawner spawner in GameObject.FindObjectsOfType<BriefSpawner>())
        {
            spawners.Add(spawner);
        }
        NextStep();
    }
    void NextStep()
    {
        baseBrief += 5;
        basePlane += 1;
        briefenToSpawn = baseBrief;
        planesToSpawn = basePlane;
        Wave();
    }

    void Wave()
    {
        while (briefenToSpawn != 0)
        {
            SpawnBrief();
            briefenToSpawn -= 1;            
        }
        
        while (planesToSpawn != 0)
        {
            SpawnPlane();
            planesToSpawn -= 1;            
        }
    }


    void SpawnBrief()
    {
        int randomSpawner = Random.Range(0,spawners.Count);
        spawners[randomSpawner].SpawnBrief(0);
    }

    void SpawnPlane()
    {
        int randomSpawner = Random.Range(0,spawners.Count);
        spawners[randomSpawner].SpawnBrief(1);
    }

    public void CheckForEnemies()
    {
        Invoke("NextStep",5);
    }
}
