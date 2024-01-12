using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace StateMachine
{
    public class Player : MonoBehaviour
    {
        #region variables and properties

        public string currentState; //debug

        //linked classes
        public StateMachine stateMachine;
        public PlayerController playerController;
        public CharacterController controller;
        public Transform handsocket;
        public PalloController heldPallo;

        //movement
        [SerializeField] public float playerSpeed = 5f;
        [SerializeField] public float slowPlayerSpeed = 2f;
        [SerializeField] public float dodgePlayerSpeed = 13f;
        public Vector3 movementInput = Vector3.zero; //TODO put private
        public Vector3 rotationInput = Vector3.zero; //TODO put private

        //throw
        [SerializeField] public float currentChargeTime = 0;
        [SerializeField] public float maxChargeTime = 1;
        [SerializeField] public float maxThrowForce = 10;
        [SerializeField] public float throwHeight = 1.2f;
        [SerializeField] public float holdBallCooldown = 0;
        [SerializeField] public float holdBallCooldownDuration = 2f;
        public float MinThrowForce => PalloController.SPEED_TIERS[1];

        //dodge
        public Vector3 dodgeDirection;
        public float dodgeCurrentSpeed = 0f;
        public float dodgeMaxDuration = .5f;
        public float dodgeCurrentTime = 0f;

        //hp
        public int currentHp;
        public int CurrentHp
        {
            get { return currentHp; }

            set
            {
                if (value < 0)
                    currentHp = 0;
                else
                    currentHp = value;
            }
        }

        //properties
        public bool IsHoldingBall => heldPallo;
        public bool IsChargingShot => currentChargeTime != 0;
        public Vector3 ThrowVelocity
        {
            get
            {
                return transform.forward *
                (MinThrowForce + (Mathf.Min(currentChargeTime, maxChargeTime)
                * (maxThrowForce - MinThrowForce) / maxChargeTime))
                + Vector3.up * throwHeight;
            }
        }

        #endregion

        void Start()
        {
            stateMachine = new StateMachine(this);

            GetComponent<PalloTrigger>().AddOnEnterListener(PalloContact);
            controller = gameObject.GetComponent<CharacterController>();
            playerController = gameObject.GetComponent<PlayerController>();
        }

        void Update()
        {
            stateMachine.currentState?.Update();
        }

        private void PalloContact(PalloController pallo)
		{
            /*
            if (holdBallCooldown <= 0)
            {
                
            }*/
            heldPallo = pallo;
            heldPallo.Hold(handsocket);
        }

        public void TakeDamage(int amount)
        {
            CurrentHp -= amount;

            Debug.Log("Player HP = " + CurrentHp);

            if (CurrentHp <= 0)
            {
                this.KillPlayer();
            }
        }

        //TODO trasformare in unityEvent
        public void KillPlayer()
        {
            Debug.Log("player is killed!");
            //TODO rimuovi destroy e togli una vita
            Destroy(this.gameObject);
        }
    }
}

