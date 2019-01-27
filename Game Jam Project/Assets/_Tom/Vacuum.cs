using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    [SerializeField] private float pullForce = 300f;
    
    private Vector3 forceDirection;
    private Rigidbody rb;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Letter"))
        {
            forceDirection = transform.position - other.transform.position;

            rb = other.transform.GetComponent<Rigidbody>();

            rb.AddForce(forceDirection.normalized * pullForce * Time.deltaTime);
        }
    }
}
