using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField] protected float duration = 5f; 

    protected bool activated;

    private void OnTriggerStay(Collider other)
    {
        if (activated)
        {
            if (other.CompareTag("Letter"))
            {
                Effect(other.GetComponent<Letter>());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                activated = true;
                StartCoroutine(SetDuration());
            }
        }
    }

    public virtual IEnumerator SetDuration()
    {
        yield return new WaitForSeconds(duration);
        activated = false;
    }

    public abstract void Effect(Letter letter);
}