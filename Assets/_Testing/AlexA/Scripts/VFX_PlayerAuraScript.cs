using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using DG.Tweening;

public class VFX_PlayerAuraScript : MonoBehaviour
{
    [SerializeField] private List<Material> materials;
    
    private GameObject vfx_PlayerAuraCapsule;
    private GameObject vfx_PlayerAuraBase;

    private Player myPlayer;

    private float maxSize;
    private float minSize;
    private float currentSize;
    private bool isCharging;

    private float CurrentSize 
    {
        get { return currentSize; }
        set 
        {
            if (value < minSize)
            {
                currentSize = minSize;
            }
            else if (value > maxSize)
            {
                currentSize = maxSize;
            }
            else
                currentSize = value;
        }
    }

    private void OnDisable()
    {
        myPlayer.stateMachine.throww.OnEnter -= OnChargingStarted;
        myPlayer.stateMachine.throww.OnExit -= OnChargingCancelled;
    }

    private void Update()
    {
        OnChargingUpdate();
    }

    private void Init()
    {
        myPlayer = gameObject.GetComponentInParent<Player>();
        myPlayer.stateMachine.throww.OnEnter += OnChargingStarted;
        myPlayer.stateMachine.throww.OnExit += OnChargingCancelled;
        vfx_PlayerAuraCapsule = transform.Find("PlayerAuraCapsule").gameObject;
        vfx_PlayerAuraBase = transform.Find("PlayerAuraBase").gameObject;
    }

    private void CheckThrowLevel(int level)
    {
        switch (level) 
        {
            case 0:
                vfx_PlayerAuraCapsule.GetComponent<ParticleSystemRenderer>().material = materials[0];
                vfx_PlayerAuraBase.GetComponent<ParticleSystemRenderer>().material = materials[0];
                break;
            case 1:
                vfx_PlayerAuraCapsule.GetComponent<ParticleSystemRenderer>().material = materials[1];
                vfx_PlayerAuraBase.GetComponent<ParticleSystemRenderer>().material = materials[1];
                break;
            case 2:
                vfx_PlayerAuraCapsule.GetComponent<ParticleSystemRenderer>().material = materials[2];
                vfx_PlayerAuraBase.GetComponent<ParticleSystemRenderer>().material = materials[2];
                break;
            case 3:
                vfx_PlayerAuraCapsule.GetComponent<ParticleSystemRenderer>().material = materials[3];
                vfx_PlayerAuraBase.GetComponent<ParticleSystemRenderer>().material = materials[3];
                break;
        }
    }

    private void OnChargingStarted(Player owner) 
    {
        /*int throwLevel = player.heldPallo.speedTier;
         *CheckThrowLevel(throwLevel);*/
        vfx_PlayerAuraCapsule.SetActive(true);
        vfx_PlayerAuraBase.SetActive(true);
        isCharging = true;
    }
    
    private void OnChargingUpdate() 
    {
        if (isCharging) 
        { 
            vfx_PlayerAuraCapsule.transform.DOScale(maxSize, myPlayer.maxChargeTime);
        }
    }

    private void OnChargingCancelled(Player owner) 
    {
        isCharging = false;
        vfx_PlayerAuraCapsule.SetActive(false);
        vfx_PlayerAuraBase.SetActive(false);
        vfx_PlayerAuraCapsule.transform.DOScale(minSize, 0f);
    }
}
