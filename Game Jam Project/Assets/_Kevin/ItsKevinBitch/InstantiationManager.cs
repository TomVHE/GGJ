using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InstantiationManager : MonoBehaviour
{
   public List<GameObject> objects = new List<GameObject>();
   public static InstantiationManager instance;

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

   /// <summary>
   /// Call to instantiate Enemy
   /// </summary>
   /// <param name="pos"> Position of Enemy</param>
   /// <param name="name"> Name of Enemy object in list</param>
   public void InstantiateEnemyType(Transform pos, int range)
   {
      WaveSystem.aliveEnemies += 1;
      Instantiate(objects[range],pos.position,Quaternion.identity);
   }
}
