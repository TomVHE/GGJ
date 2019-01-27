using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public ParticleActive activateParticle;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Letter"))
        {
            activateParticle.Activate();
            other.transform.GetComponent<Letter>().Death();
        }
    }
}
