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
   /// Call to instantiate entity
   /// </summary>
   /// <param name="pos"> Position of entity</param>
   /// <param name="name"> Name of entity object in list</param>
   public void InstantiateEntity(Transform pos, int type)
   {
      print(type);
      Instantiate(objects[type],pos.position,Quaternion.identity);
   }

   public void InstantiateEnemyType(Transform pos, int range)
   {
      int index = Random.Range(0,range);
   }
}
