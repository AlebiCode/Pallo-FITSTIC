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

            components = new ParticleSystem[transform.childCount];
            for (int i = 0; i < components.Length; i++)
            {
                components[i] = transform.GetChild(i).gameObject.GetComponentInChildren<ParticleSystem>();
            }

            //components = GetComponentsInChildren<ParticleSystem>();
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
            //transform.parent = null;
            foreach (ParticleSystem p in components)
            {
                p.gameObject.SetActive(true);
                p.GetComponent<ParticleSystem>().Play();
            }

            yield return new WaitForSeconds(2f);

            foreach (ParticleSystem p in components)
            {
                p.GetComponent<ParticleSystem>().Stop();
                p.gameObject.SetActive(false);
            }

            //transform.parent = parent;
            //transform.position = transform.parent.position;
        }
    }
}
