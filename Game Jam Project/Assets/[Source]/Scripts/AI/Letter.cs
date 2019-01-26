using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Letter : NavAgent
{
    [SerializeField] private float happinessGain = -10f;

    private Transform target;

    private void Start()
    {
        target = Parent.Instance.transform;
    }

    private void Update()
    {
        MoveTo(target.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Parent"))
        {
            Parent.Instance.Happiness += happinessGain;
            Destroy(gameObject);
        }
    }

    public void Death()
    {
        GameManager.Instance.Score++;
        Destroy(gameObject);
    }
}
