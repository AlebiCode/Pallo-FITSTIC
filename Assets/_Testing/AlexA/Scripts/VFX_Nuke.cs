using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace VFX 
{ 
    public class VFX_Nuke : MonoBehaviour, IExplosionVFX
    {
        private Transform[] components;
        private Transform parent;

        private PalloController myPallo;

        private bool isOnPallo = false;
        private bool isListeningToEvents = false;

        void Start()
        {
            Init();
            StartListeningToMyPallo();
        }

        private void OnEnable()
        {
            StartListeningToMyPallo();
        }
        private void OnDisable() 
        {
            StopListeningToMyPallo();
        }
        private void Init() 
        { 
            components = new Transform[transform.childCount];
            for (int i = 0; i < components.Length; i++)
            {
                components[i] = transform.GetChild(i);
            }
            parent = transform.parent;
            if (transform.parent.TryGetComponent(out myPallo)) 
            { 
                isOnPallo = true;
            }
        }

        private void StartListeningToMyPallo() 
        { 
            if(isOnPallo && !isListeningToEvents) 
            {
                myPallo.BallBlowUp.AddListener(Explode);
                isListeningToEvents = true;
            }
        }

        private void StopListeningToMyPallo() 
        {
            if (isOnPallo && isListeningToEvents)
            {
                myPallo.BallBlowUp.RemoveListener(Explode);
                isListeningToEvents = false;
            }
        }

        public void Explode()
        {
            StartCoroutine(ExplodeCoroutine());
        }

        public IEnumerator ExplodeCoroutine() 
        {
            transform.parent = null;
            foreach(Transform t in components) 
            { 
                t.gameObject.SetActive(true);
                t.GetComponent<ParticleSystem>().Play();
            }

            yield return new WaitForSeconds(2f);

            foreach(Transform t in components) 
            {
                t.GetComponent<ParticleSystem>().Stop();
                t.gameObject.SetActive(false);
            }

            transform.parent = parent;
            transform.position = transform.parent.position;
        }
    }
}
