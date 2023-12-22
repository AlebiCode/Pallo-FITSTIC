using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Rigidbody))]

    public class PlayerController : MonoBehaviour
    {
        //refactoring assolutamente necessario per questo vomito di variabili
        private CharacterController controller;
        private Vector3 playerVelocity;
        [SerializeField] private float playerSpeed = 5f;
        [SerializeField] private float slowPlayerSpeed = 2f;
        [SerializeField] private float currentPlayerSpeed;
        private Vector3 movementInput = Vector3.zero;
        private Vector3 rotationInput = Vector3.zero;
        private bool dodging = false;
        private PalloController heldPallo;
        [SerializeField] private float currentChargeTime = 0;
        [SerializeField] private float maxChargeTime = 1;
        [SerializeField] private float maxThrowForce = 10;
        private float MinThrowForce => PalloController.SPEED_TIERS[1];
        [SerializeField] private float throwHeight = 1.2f;
        [SerializeField] private Transform handsocket;
        [SerializeField] private float holdBallCooldown = 0;
        [SerializeField] private float holdBallCooldownDuration = 2f;

        private int currentHp;
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

        private bool IsHoldingBall => heldPallo;
        private bool IsChargingShot => currentChargeTime != 0;
        private Vector3 ThrowVelocity
		{
            get
			{
                return transform.forward *
                (MinThrowForce + (Mathf.Min(currentChargeTime, maxChargeTime)
                * (maxThrowForce - MinThrowForce) / maxChargeTime))
                + Vector3.up * throwHeight;
            }
		}

        private void Start()
        {
            controller = gameObject.GetComponent<CharacterController>();
            currentPlayerSpeed = playerSpeed;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            movementInput = new Vector3(
                context.ReadValue<Vector2>().x, 
                0,
                (context.ReadValue<Vector2>().y));
        }

        public void OnRotation(InputAction.CallbackContext context)
        {
            rotationInput = new Vector3(
                context.ReadValue<Vector2>().x,
                0,
                (context.ReadValue<Vector2>().y));
        }

        public void OnThrow(InputAction.CallbackContext context)
		{
            if (!IsHoldingBall)
			{
                Debug.Log("No ball to throw");
                return;
			}

            if (context.phase == InputActionPhase.Canceled)
            {
                heldPallo.Throw(ThrowVelocity);
                heldPallo = null;
                currentChargeTime = 0;
                currentPlayerSpeed = playerSpeed;
                holdBallCooldown = holdBallCooldownDuration;
                Debug.Log("Throw complete");
            }
			else
			{
                currentPlayerSpeed = slowPlayerSpeed;
            }
        }

        public void OnParry(InputAction.CallbackContext context)
        {

        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            dodging = true;
        }

        void Update()
        {
            if (dodging)
            {
                HandleDodge();
                return;
            }
            HandleMovement();
            HandleRotation();
            HandleThrow();
            if (!IsHoldingBall && holdBallCooldown > 0)
                holdBallCooldown -= Time.deltaTime;
        }

        private void HandleMovement()   
        {
            if (playerVelocity.y < 0) //TODO && !stoVenendoSpinto
                playerVelocity.y = 0f;

            controller.Move(movementInput * Time.deltaTime * currentPlayerSpeed);
        }

        private void HandleRotation()
        {
            //if can charge ball
            if (heldPallo && rotationInput != Vector3.zero)
            {
                Vector3 rotationVector = transform.position + rotationInput;
                transform.LookAt(rotationVector, Vector3.up);
            }
			else if (movementInput != Vector3.zero)
			{
                Vector3 moveVector = transform.position + movementInput;
                transform.LookAt(moveVector, Vector3.up);
            }
        }

        private void HandleThrow()
		{
            if (IsHoldingBall)
            {
                currentChargeTime += Time.deltaTime;
                Debug.Log("currentChargeTime = " + currentChargeTime);
            }
        }

        private void HandleDodge()
        {
            dodging = false;
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