using System.Collections;
using System.Collections.Generic;
using Core.Movement;
using Core.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using KinematicCharacterController.Walkthrough.NavMeshExample;

public class InteractionManager : MonoBehaviour
{
    #if UNITY_EDITORs
    [Required]
    #endif
    [SerializeField] private AI pncPlayer = null;
    //[SerializeField] private MyAI pncPlayer = null;

    private Vector3 destination = Vector3.zero;

    [SerializeField] private Camera m_camera = null;
    
    private void Start()
    {
        if (m_camera == null)
        {
            m_camera = Camera.main;
        }
    }

    private void Update()
    {
            RaycastHit hit;
            
            if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                //destination = hit.point;
                DebugExtension.DebugArrow(hit.point, Vector3.up, Color.red);

                if (Input.GetMouseButton(0))
                {
                    pncPlayer.destination = hit.point;
                }
            }
    }

    private void OnDrawGizmos()
    {
           
    }
}
