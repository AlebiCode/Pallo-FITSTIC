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
        public float controllerDeadzone = 0.1f;
        public float regularSpeed = 5f;
        public float slowSpeed = 2f;
        public float dodgeSpeed = 15f;
        public Vector2 movementInput = Vector2.zero;
        public Vector2 rotationInput = Vector2.zero;

        private float rotateSmoothing = 3f;

        //throw
        [SerializeField] public float currentChargeTime = 0;
        [SerializeField] public float maxChargeTime = 1;
        [SerializeField] public float maxThrowForce = 10;
        [SerializeField] public float throwHeight = 1.2f;
        [SerializeField] public float holdBallCooldown = 0;
        [SerializeField] public float holdBallCooldownDuration = 2f;

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

        public Vector3 MovementDirectionFromInput => new Vector3(movementInput.x, 0, movementInput.y).normalized;
        private bool GottaMove => Mathf.Abs(movementInput.x) > controllerDeadzone ||
                                 Mathf.Abs(movementInput.y) > controllerDeadzone ||
                                 stateMachine.currentState == stateMachine.dodge ||
                                 stateMachine.currentState == stateMachine.stun;
        public bool IsHoldingBall => heldPallo;
        public float MinThrowForce => PalloController.SPEED_TIERS[1];
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

        void Awake()
        {
            stateMachine = new StateMachine(this);

            GetComponent<PalloTrigger>().AddOnEnterListener(PalloContact);
            playerController = gameObject.GetComponent<PlayerController>();
            controller = gameObject.GetComponent<CharacterController>();
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            handsocket = GameObject.Find("HandSocket").GetComponentInChildren<Transform>(); 
        }

        void Update()
        {
            stateMachine.currentState?.Update();
        }

		#region helper methods

		public void HandleMovement(Vector3 direction, float speed)
        {
            if (GottaMove)
			{
                controller.Move(direction * speed * Time.deltaTime);
            }
        }

        public void HandleRotation()
        {
            if (usingGamePad)
            {
                GamepadRotation();
            }
            else
            {
                KeyboardRotation();
            }
        }

        private void GamepadRotation()
        {
            if (Mathf.Abs(rotationInput.x) > controllerDeadzone || Mathf.Abs(rotationInput.y) > controllerDeadzone)
            {
                Vector3 playerDirection = Vector3.right * rotationInput.x + Vector3.forward * rotationInput.y;
                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSmoothing * Time.deltaTime);
                }
            }
            else if (Mathf.Abs(movementInput.x) > controllerDeadzone || Mathf.Abs(movementInput.y) > controllerDeadzone)
            {
                transform.LookAt(transform.position + MovementDirectionFromInput, Vector3.up);
            }
        }

        private void KeyboardRotation()
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(rotationInput);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
                LookAt(pointToLook);
            }
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


        public void KillPlayer()
        {
            //TODO trasformare in unityEvent

            Debug.Log("player is killed!");
            //TODO rimuovi destroy e togli una vita
            Destroy(this.gameObject);
        }

		#endregion
	}
}

