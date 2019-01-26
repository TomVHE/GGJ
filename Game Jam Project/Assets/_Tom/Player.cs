using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NavAgent
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
            {
                MoveTo(hit.point);
            }
        }
        
    }
}
