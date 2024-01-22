using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_SimpleBurstVFX : MonoBehaviour
{
    private ParticleSystem[] vfxComponents;

    private void Awake()
    {
        Init();
    }

    private void Init() 
    {
        vfxComponents = gameObject.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem p in vfxComponents)
        {
            p.Stop();
            p.gameObject.SetActive(false);
        }
    }

    public void Explode() 
    {
        foreach (ParticleSystem p in vfxComponents)
        {
            p.gameObject.SetActive(true);
            p.Play();
        }
    }
}
