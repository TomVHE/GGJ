using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriefSpawner : MonoBehaviour
{
    public void SpawnBrief(int amount, int type)
    {
        int toSpawn = amount;
        while(toSpawn != 0)
        {
            toSpawn -= 1;
            InstantiationManager.instance.InstantiateEntity(gameObject.transform,type);
        }
    }
}
