using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriefSpawner : MonoBehaviour
{
    public void SpawnBrief(int type)
    {
        InstantiationManager.instance.InstantiateEnemyType(gameObject.transform, type);
    }
}
