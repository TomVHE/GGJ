using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActive : MonoBehaviour
{
    public ParticleSystem particle;
    
    public void Activate()
    {
        particle.Play();
    }
}
