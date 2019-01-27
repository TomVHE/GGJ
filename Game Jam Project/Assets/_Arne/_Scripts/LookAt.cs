using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform camTransform;
    	
	// Use this for initialization
	void Start () 
	{	
		camTransform = Camera.main.transform;
	}
	// Update is called once per frame
	void FixedUpdate () 
	{
        Vector3 targetPostition = new Vector3(camTransform.position.x, this.transform.position.y, camTransform.position.z);
        this.transform.LookAt(targetPostition);
	}
}
