using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace StateMachine
{
    public class Player : MonoBehaviour
    {
        #region variables and properties
        //TODO refactoring assolutamente necessario per questo vomito di variabili e proprietà

        //linked classes
        public StateMachine stateMachine;
        public PlayerController playerController;
        public CharacterController controller;
        public PalloController heldPallo;

        public string currentState;

        //movement
        public Vector3 playerVelocity;
        [SerializeField] public float playerSpeed = 5f;
        [SerializeField] public float slowPlayerSpeed = 2f;
        [SerializeField] public float currentPlayerSpeed;
        public Vector3 movementInput = Vector3.zero;
        public Vector3 rotationInput = Vector3.zero;

        //throw
        [SerializeField] public float currentChargeTime = 0;
        [SerializeField] public float maxChargeTime = 1;
        [SerializeField] public float maxThrowForce = 10;
        [SerializeField] public float throwHeight = 1.2f;
        [SerializeField] public Transform handsocket;
        [SerializeField] public float holdBallCooldown = 0;
        [SerializeField] public float holdBallCooldownDuration = 2f;
        public float MinThrowForce => PalloController.SPEED_TIERS[1];

        //dodge
        public bool dodging = false;
        public float dodgeDuration = .25f;
        public float dodgeCounter = 0f;

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
            //versione monobehaviour
            //stateMachine = (gameObject.AddComponent(typeof(StateMachine)) as StateMachine);
            //stateMachine.Initialize(this);

            //versione non monobehaviour
            stateMachine = new StateMachine(this);

            controller = gameObject.GetComponent<CharacterController>();
            playerController = gameObject.GetComponent<PlayerController>();
        }

        void Update()
        {
            stateMachine.currentState?.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (holdBallCooldown <= 0)
            {
                heldPallo = other.GetComponent<PalloController>();
            }

            if (IsHoldingBall)
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

