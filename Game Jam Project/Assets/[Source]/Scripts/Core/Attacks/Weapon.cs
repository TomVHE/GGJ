using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Sirenix.OdinInspector;

public class Weapon : MonoBehaviour
{
    #if UNITY_EDITOR
    [Required]
    #endif
    [SerializeField] private CinemachineFreeLook m_cinemachineFreeLook = null;
    
    #if UNITY_EDITOR
    [MinMaxSlider(-180, 180, true)]
    #endif
    [SerializeField] private Vector2 minMaxAttackAngle = new Vector2(-90, 90);
    
    //#if UNITY_EDITOR
    //[MinValue(-1), MaxValue(1)]
    //#endif
    //[SerializeField] private float currentAttackAngle = 0;

    private void Start()
    {
        if (m_cinemachineFreeLook == null)
        {
            m_cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();

            if (m_cinemachineFreeLook == null)
            {
                Debug.LogError("No CinemachineFreeLook Assigned, you dipshit!");
            }
        }
    }

    private void Update()
    {
        float attackAngle = Mathf.Lerp(minMaxAttackAngle.x, minMaxAttackAngle.y, 1 - m_cinemachineFreeLook.m_YAxis.Value);

        Vector3 myRot = transform.eulerAngles; 
        transform.eulerAngles = new Vector3(myRot.x, myRot.y, attackAngle);
    }
}
