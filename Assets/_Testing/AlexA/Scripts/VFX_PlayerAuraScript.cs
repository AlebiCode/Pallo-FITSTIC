using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using DG.Tweening;
using Controllers;
using LorenzoCastelli;

namespace VFX
{ 
    public class VFX_PlayerAuraScript : MonoBehaviour
    {
        [SerializeField] private List<Material> materials;
    
        private GameObject vfx_PlayerAuraCapsule;
        private GameObject vfx_PlayerAuraBase;

        private PlayerData myPlayer;
        private bool isPlayer = false;

        [SerializeField]private float maxSize;
        [SerializeField]private float minSize;
        private float currentSize;

        private bool isCharging;
        private bool isInitialized = false;
        private bool isListeningToEvents = false;

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


        private void OnEnable()
        {
            Init();
        }
        private void OnDisable()
        {
            StopListeningToMyPlayer();
        }
        
        private void Update()
        {
            OnChargingUpdate();
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            StartCoroutine(InitCoroutine());
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
            CheckThrowLevel(myPlayer.HeldPallo.SpeedTier);
            vfx_PlayerAuraCapsule.SetActive(true);
            vfx_PlayerAuraBase.SetActive(true);
            isCharging = true;
        }
    
        private void OnChargingUpdate() 
        {
            if (isCharging) 
            {
                vfx_PlayerAuraCapsule.transform.localScale = Vector3.Lerp(Vector3.one * minSize, Vector3.one * maxSize, myPlayer.throwChargeMax);
            }
        }

        private void OnChargingCancelled(Player owner) 
        {
            isCharging = false;
            vfx_PlayerAuraCapsule.SetActive(false);
            vfx_PlayerAuraBase.SetActive(false);
            vfx_PlayerAuraCapsule.transform.DOScale(minSize, 0f);
        }

        private void StartListeningToMyPlayer() 
        {
            if ((!isListeningToEvents) && (isPlayer)) 
            {
                Player player = myPlayer.GetComponent<Player>();
                player.StateMachine.aimthrow.OnEnter += OnChargingStarted;
                player.StateMachine.aimthrow.OnExit += OnChargingCancelled;
                isListeningToEvents = true;
            }
        }
        private void StopListeningToMyPlayer()
        {
            if ((isListeningToEvents) && (isPlayer))  
            {
                Player player = myPlayer.GetComponent<Player>();
                player.StateMachine.aimthrow.OnEnter -= OnChargingStarted;
                player.StateMachine.aimthrow.OnExit -= OnChargingCancelled;
                isListeningToEvents = false;
            }
        }

        private IEnumerator InitCoroutine() 
        {
            if (!isInitialized)
            {
                myPlayer = gameObject.GetComponentInParent<PlayerData>();
                if (myPlayer.gameObject.GetComponent<Player>()) {
                    isPlayer = true;
                }
                vfx_PlayerAuraCapsule = transform.Find("PlayerAuraCapsule").gameObject;
                vfx_PlayerAuraBase = transform.Find("PlayerAuraBase").gameObject;
                isInitialized = true;
            }

            yield return new WaitForEndOfFrame();

            if(transform.parent.TryGetComponent(out myPlayer)) 
            { 
                StartListeningToMyPlayer();
            }

        }
    }
}
