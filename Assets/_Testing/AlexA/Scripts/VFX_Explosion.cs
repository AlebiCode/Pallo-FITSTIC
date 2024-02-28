using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VFX 
{ 
    public class VFX_Explosion : MonoBehaviour, IExplosionVFX
    {
        private ParticleSystem[] components;
        private Transform parent;

        private void Start()
        {
            components = GetComponentsInChildren<ParticleSystem>();
        }

        private void Init() 
        {
            components = GetComponentsInChildren<ParticleSystem>();
        }

        public void Explode()
        {
            StartCoroutine(ExplodeCoroutine());
        }

        public IEnumerator ExplodeCoroutine()
        {
            transform.parent = null;
            foreach (ParticleSystem p in components)
            {
                p.gameObject.SetActive(true);
                p.Play();
            }

            yield return new WaitForSeconds(2f);

            foreach (ParticleSystem p in components)
            {
                p.Stop();
                p.gameObject.SetActive(false);
            }

            transform.parent = parent;
            transform.position = transform.parent.position;
        }
    }
}
