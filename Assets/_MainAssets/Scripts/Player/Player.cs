using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Controllers;

namespace StateMachine
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PalloTrigger))]

    public class Player : MonoBehaviour
    {
        #region variables and properties

        public string currentState; //debug

        //linked classes
        private StateMachine        stateMachine;
        private PlayerController    playerController;
        private CharacterController controller;
        private Transform           handsocket;
        private PalloController     heldPallo;
        private Camera              mainCamera;

        //hp
        private Vector3 pushDirection;
        private float pushForce;
        private float pushDeterioration = 2f;
        public int currentHp;
        public LayerMask palloLayermask;

        [Header("Movement")]  
        public float    speedRegular = 5f;
        public float    speedSlow = 2f;
        public float    speedDodge = 15f;
        public Vector2  movementInput = Vector2.zero;
        public Vector2  rotationInput = Vector2.zero;
        public bool     usingGamePad;
        public float    controllerDeadzone = 0.01f;
        private float   rotateSmoothing = 3f;

        [Header("Throw")]
        public float throwChargeMax = 1f;
        public float holdBallCooldown = 2f;
        private float throwChargeCurrent = 0f;
        private float throwForceMax = 10f;
        private float throwHeight = 1.2f;
        private float holdBallCooldownCurrent = 0f;

        [Header("Dodge")]
        public float dodgeDuration = .6f; //durata dodge
        public float dodgeCooldown = 1f; //durata dodge cooldown
		private float dodgeCooldownCurrent = 0f;

        [Header("Parry")]
        public float parryDuration = 0.5f; //quanto a lungo dura l'azione del parry
        public float parryPercentage = 60f; //percentuale del parry in cui sei invincibile
        public float parryRange = 2f; //quanto lontano raggiunge il parry
        public float parryCooldown = 1f;
        public bool isInvincibile = false;
        private float parryCooldownCurrent = 0f;

        //properties
        public StateMachine StateMachine            { get => stateMachine; set => stateMachine = value; }
        public PlayerController PlayerController    { get => PlayerController; set => PlayerController = value; }
        public PalloController HeldPallo            { get => heldPallo; set => heldPallo = value; }
		public Transform Handsocket                 { get => handsocket; set => handsocket = value; }
        public float DodgeCooldownCurrent           { get => dodgeCooldownCurrent; set => dodgeCooldownCurrent = value; }
        public float ParryCooldownCurrent           { get => parryCooldownCurrent; set => parryCooldownCurrent = value; }
        public float HoldBallCooldownCurrent        { get => holdBallCooldownCurrent; set => holdBallCooldownCurrent = value; }
        public float ThrowChargeCurrent             { get => throwChargeCurrent; set => throwChargeCurrent = value; }
		public bool IsInvincibile                   { get => isInvincibile; set => isInvincibile = value; }
		public Vector3 PushDirection                { get => pushDirection; set => pushDirection = value; }
		public float PushForce                      { get => pushForce; set => pushForce = value; }
		public float PushDeterioration              { get => pushDeterioration; set => pushDeterioration = value; }
        public Vector3  ThrowVelocity
        {
            get
            {
                return transform.forward *
                (ThrowForceMin + (Mathf.Min(throwChargeCurrent, throwChargeMax)
                * (throwForceMax - ThrowForceMin) / throwChargeMax))
                + Vector3.up * throwHeight;
            }
        }
        public int      CurrentHp
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
        private bool IsMovementValid =>             Mathf.Abs(movementInput.x) > controllerDeadzone ||
                                                    Mathf.Abs(movementInput.y) > controllerDeadzone ||
                                                    StateMachine.currentState == StateMachine.dodge ||
                                                    StateMachine.currentState == StateMachine.stun;
        public Vector3  MovementDirectionFromInput => new Vector3(movementInput.x, 0, movementInput.y).normalized;
        public float    ThrowForceMin => PalloController.SPEED_TIERS[1];
        public bool     IsHoldingBall => heldPallo;

		#endregion

		void Awake()
        {
            stateMachine = new StateMachine(this);

            GetComponent<PalloTrigger>().AddOnEnterListener(PalloContact);
            playerController = gameObject.GetComponent<PlayerController>();
            controller = gameObject.GetComponent<CharacterController>();
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            Handsocket = GameObject.Find("HandSocket").GetComponentInChildren<Transform>(); 
        }

        void Update()
        {
            stateMachine.currentState?.Update();

            HandleCooldowns();
        }

        #region helper methods

        private void HandleCooldowns()
        {
            //dodge cooldown
            if (DodgeCooldownCurrent > 0)
            {
                DodgeCooldownCurrent -= Time.deltaTime;
            }

            //parry cooldown
            if (ParryCooldownCurrent > 0)
            {
                ParryCooldownCurrent -= Time.deltaTime;
            }

            //hold ball cooldown
            if (HoldBallCooldownCurrent > 0 && !IsHoldingBall)
			{
                HoldBallCooldownCurrent -= Time.deltaTime;
            }
        }

        public void HandleMovement(Vector3 direction, float speed)
        {
            if (IsMovementValid)
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
			switch (pallo.GetBallState)
			{
                case PalloController.BallStates.thrown:
                    Debug.Log("Pushed");
                    GetPushed(
                        pallo.GetComponent<Rigidbody>().velocity.normalized,
                        pallo.SpeedTier);
                    Debug.Log("Speedtier: " + pallo.SpeedTier);
                    break;

                case PalloController.BallStates.bouncing:
                    Debug.Log("Pick up");
                    if (holdBallCooldown <= 0)
                    {
                        heldPallo = pallo;
                        heldPallo.Hold(Handsocket);
                    }
                    break;
            }
        }

        private void GetPushed(Vector3 pushDirection, float pushForce)
		{
            if (!IsInvincibile)
			{
                PushDirection = pushDirection;
                PushForce = pushForce;
                stateMachine.ChangeState(stateMachine.stun);
			}
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

