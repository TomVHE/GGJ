using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Sirenix.OdinInspector;

public class Weapon : MonoBehaviour
{
    private enum RotationType
    {
        AimTowardScreenCenter,
        Constrained
    }

    #if UNITY_EDITOR
    [EnumPaging]
    #endif
    [SerializeField] private RotationType rotationType = RotationType.Constrained;

    #region Contrained Rotations
    
    #if UNITY_EDITOR
    [Required, 
     Space(10), ShowIf("rotationType", RotationType.Constrained)]
    #endif
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook = null;
    
    #if UNITY_EDITOR
    [MinMaxSlider(-180, 180, true), 
     ShowIf("rotationType", RotationType.Constrained)]
    #endif
    [SerializeField] private Vector2 minMaxAttackAngle = new Vector2(-90, 90);
    

    #endregion

    private void Start()
    {
        if (cinemachineFreeLook == null)
        {
            cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();

            if (cinemachineFreeLook == null)
            {
                Debug.LogError("No CinemachineFreeLook Assigned, you dipshit!");
            }
        }
    }

    private void Update()
    {
        if (rotationType == RotationType.Constrained)
        {
            float attackAngle = Mathf.Lerp(minMaxAttackAngle.x, minMaxAttackAngle.y,
                1 - cinemachineFreeLook.m_YAxis.Value);

            Vector3 myRot = transform.eulerAngles;
            transform.eulerAngles = new Vector3(myRot.x, myRot.y, attackAngle);
        }
    }
}
