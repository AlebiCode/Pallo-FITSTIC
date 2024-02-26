using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class VFX_Parry : MonoBehaviour
{
    private Player myPlayer;
    private ParticleSystem[] components;

    private bool isInitialized = false;
    private bool isListeningToEvents = false;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        StopListeningToMyPlayer();
    }

    private void Init() 
    { 
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine() 
    { 
        if (!isInitialized) 
        { 
            components = GetComponentsInChildren<ParticleSystem>();
            isInitialized = true;
        }
        yield return new WaitForEndOfFrame();
        if (transform.parent.TryGetComponent(out myPlayer))
        {
            StartListeningToMyPlayer();
        }
    }

    private void StartListeningToMyPlayer() 
    {
        if(!isListeningToEvents) 
        { 
            myPlayer.StateMachine.parry.OnEnter += Parry;
            isListeningToEvents = true;
        }
    }

    private void StopListeningToMyPlayer()
    {
        if (isListeningToEvents) 
        { 
            myPlayer.StateMachine.parry.OnEnter -= Parry;
            isListeningToEvents = false;
        }
    }

    private void Parry(Player owner) 
    { 
        foreach (var p in components) 
        { 
            p.Play();
        }
    }

}
