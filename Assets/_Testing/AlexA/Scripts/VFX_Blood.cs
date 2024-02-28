using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.VFX;

namespace VFX 
{ 
    public class VFX_Blood : MonoBehaviour
    {
        private Player myPlayer;

        private VisualEffect[] components;

        private bool isInitialized = false;
        private bool isListeningToEvents = false;

        private void OnEnable()
        {
            Init();
        }
        private void OnDisable()
        {
            StopListeningToMyPlayer();
        }
        private void Start()
        {
            Init();
        }
        private void Init() 
        {
            StartCoroutine(InitCoroutine());
        }
        private void GetHitEnter(Player owner) 
        { 
            foreach (VisualEffect component in components) 
            {
                component.Play();
            }
        }
        private void StartListeningToMyPlayer() 
        {
            if (!isListeningToEvents) 
            { 
                myPlayer.StateMachine.stun.OnEnter += GetHitEnter;
                isListeningToEvents = true;
            }
        }
        private void StopListeningToMyPlayer()
        {
            if (isListeningToEvents) 
            { 
                myPlayer.StateMachine.stun.OnEnter -= GetHitEnter;
                isListeningToEvents = false;
            }
        }

        private IEnumerator InitCoroutine() 
        {
            if (!isInitialized)
            {
                components = GetComponentsInChildren<VisualEffect>(true);
                foreach (VisualEffect component in components)
                {
                    component.Stop();
                    isInitialized = true;
                }
            }

            yield return new WaitForEndOfFrame();

            if (transform.parent.TryGetComponent(out myPlayer))
            {
                StartListeningToMyPlayer();
            }
        }
    }
}
