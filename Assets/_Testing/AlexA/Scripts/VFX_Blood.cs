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

        private void OnEnable()
        {
        }
        private void OnDisable()
        {
            StopListeningToMyPlayer();
        }
        private void Start()
        {
            Init();
            StartListeningToMyPlayer();
        }
        private void Init() 
        {
            if (!isInitialized) 
            {
                myPlayer = GetComponentInParent<Player>();
                components = GetComponentsInChildren<VisualEffect>(true);
                foreach(VisualEffect component in components) 
                {
                    component.Stop();
                }
                isInitialized = true;
            }
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
            myPlayer.StateMachine.stun.OnEnter += GetHitEnter;

        }
        private void StopListeningToMyPlayer()
        {
            myPlayer.StateMachine.stun.OnEnter -= GetHitEnter;

        }
    }
}
