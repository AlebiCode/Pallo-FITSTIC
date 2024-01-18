using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace StateMachine
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Animator))]

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
        public Camera mainCamera;
        public bool usingGamePad;

        //movement
        public float playerSpeed = 5f;
        public float dodgePlayerSpeed = 30f;
        public Vector2 movementInput = Vector2.zero;
        public Vector2 rotationInput = Vector2.zero;
        public Vector3 MovementDirectionFromInput => new Vector3(movementInput.x, 0, movementInput.y);

        //throw
        [SerializeField] public float currentChargeTime = 0;
        [SerializeField] public float maxChargeTime = 1;
        [SerializeField] public float maxThrowForce = 10;
        [SerializeField] public float throwHeight = 1.2f;
        [SerializeField] public float holdBallCooldown = 0;
        [SerializeField] public float holdBallCooldownDuration = 2f;
        public float MinThrowForce => PalloController.SPEED_TIERS[1];

        //dodge
        public float dodgeMaxDuration = .6f;
        public float dodgeCooldownTimer = 1f;
        public float dodgeCurrentCooldown = 0f;

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
            playerController = gameObject.GetComponent<PlayerController>();
            controller = gameObject.GetComponent<CharacterController>();
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        void Update()
        {
            stateMachine.currentState?.Update();
        }

        public void LookAt(Vector3 lookPoint)
        {
            Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
            transform.LookAt(heightCorrectedPoint);
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

