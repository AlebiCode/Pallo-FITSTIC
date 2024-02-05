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
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PalloTrigger))]
    [RequireComponent(typeof(PlayerAnimation))]

    public class Player : MonoBehaviour
    {
        #region variables and properties

        public string currentState; //debug

        //linked classes
        private StateMachine        stateMachine;
        private CharacterController controller;
        private Transform           handsocket;
        private PalloController     heldPallo;
        private Camera              mainCamera;
        [SerializeField]
        private PlayerAnimation     playerAnimation;
        public LayerMask            palloLayermask;

        //hp
        public int currentHp;

        [Header("Movement")]  
        public float    speedRegular = 5f;
        public float    controllerDeadzone = 0.01f;
        private Vector2 movementInput = Vector2.zero;
        private Vector2 rotationInput = Vector2.zero;
        private bool    usingGamePad;
        private float   rotateSmoothing = 3f;

        [Header("Throw")]
        public float speedSlow = 2f;
        public float throwChargeMax = 1f;
        public float holdBallCooldown = 2f;
        private float throwChargeCurrent = 0f;
        private float throwForceMax = 10f;
        private float throwHeight = 1.2f;
        private float holdBallCooldownCurrent = 0f;

        [Header("Dodge")]
        public float speedDodge = 15f;
        public float dodgeDuration = .6f; //durata dodge
        public float dodgeCooldown = 1f; //durata dodge cooldown
        public float dodgeDecrease = 2f; //quanto velocemente diminuisce la velocità di dodge
		private float dodgeCooldownCurrent = 0f;

        [Header("Parry")]
        public float parryDuration = 0.5f; //quanto a lungo dura l'azione del parry
        public float parryPercentage = 60f; //percentuale del parry in cui sei invincibile
        public float parryRange = 2f; //quanto lontano raggiunge il parry
        public float parryCooldown = 1f;
        public bool isInvincibile = false;
        private float parryCooldownCurrent = 0f;

        [Header("Stun")]
        public float pushFactor = 2f;
        public float pushDecrease = 2f;
        private Vector3 pushDirection;
        private float pushForce;

        //properties
        public PlayerAnimation PlayerAnimation => playerAnimation;
        public StateMachine StateMachine            { get => stateMachine; set => stateMachine = value; }
        public PlayerController PlayerController    { get => PlayerController; set => PlayerController = value; }
        public PalloController HeldPallo            { get => heldPallo; set => heldPallo = value; }
		public Transform Handsocket                 { get => handsocket; set => handsocket = value; }
		public Vector2 MovementInput                { get => movementInput; set => movementInput = value; }
		public Vector2 RotationInput                { get => rotationInput; set => rotationInput = value; }
        public float DodgeCooldownCurrent           { get => dodgeCooldownCurrent; set => dodgeCooldownCurrent = value; }
        public float ParryCooldownCurrent           { get => parryCooldownCurrent; set => parryCooldownCurrent = value; }
        public float HoldBallCooldownCurrent        { get => holdBallCooldownCurrent; set => holdBallCooldownCurrent = value; }
        public float ThrowChargeCurrent             { get => throwChargeCurrent; set => throwChargeCurrent = value; }
		public bool IsInvincibile                   { get => isInvincibile; set => isInvincibile = value; }
		public Vector3 PushDirection                { get => pushDirection; set => pushDirection = value; }
		public float PushForce                      { get => pushForce; set => pushForce = value; }
		public float PushDecrease              { get => pushDecrease; set => pushDecrease = value; }
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
        private bool IsMovementValid =>             Mathf.Abs(MovementInput.x) > controllerDeadzone ||
                                                    Mathf.Abs(MovementInput.y) > controllerDeadzone ||
                                                    StateMachine.currentState == StateMachine.dodge ||
                                                    StateMachine.currentState == StateMachine.stun;
        public Vector3  MovementDirectionFromInput => new Vector3(MovementInput.x, 0, MovementInput.y).normalized;
        public float    ThrowForceMin => PalloController.SPEED_TIERS[1];
        public bool     IsHoldingBall => heldPallo;


		#endregion

		void Awake()
        {
            stateMachine = new StateMachine(this);

            GetComponent<PalloTrigger>().AddOnEnterListener(PalloContact);
            //playerController = gameObject.GetComponent<PlayerController>();
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
                Vector3 leg = transform.InverseTransformDirection(MovementDirectionFromInput);
                PlayerAnimation.LegMovementParameters(leg.x, leg.z);
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
            if (Mathf.Abs(RotationInput.x) > controllerDeadzone || Mathf.Abs(RotationInput.y) > controllerDeadzone)
            {
                Vector3 playerDirection = Vector3.right * RotationInput.x + Vector3.forward * RotationInput.y;
                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSmoothing * Time.deltaTime);
                }
            }
            else if (Mathf.Abs(MovementInput.x) > controllerDeadzone || Mathf.Abs(MovementInput.y) > controllerDeadzone)
            {
                transform.LookAt(transform.position + MovementDirectionFromInput, Vector3.up);
            }
        }

        private void KeyboardRotation()
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(RotationInput);
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
			switch (pallo.BallState)
			{
                case PalloController.BallStates.thrown:

                    GetPushed(
                        pallo.Velocity.normalized,
                        PalloController.SPEED_TIERS[pallo.SpeedTier] * pushFactor);

                    break;

                case PalloController.BallStates.bouncing:

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

