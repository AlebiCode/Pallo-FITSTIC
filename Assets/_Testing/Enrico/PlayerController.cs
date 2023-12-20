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
        [SerializeField] private float playerSpeed = 1f;
        private Vector3 movementInput = Vector3.zero;
        private Vector3 rotationInput = Vector3.zero;
        private bool dodging = false;
        private PalloController heldPallo;
        [SerializeField] private float currentChargeTime = 0;
        [SerializeField] private float maxChargeTime = 1;
        [SerializeField] private float maxThrowForce = 10;
        private float MinThrowForce => PalloController.TIER_2_SPEED;
        [SerializeField] private Transform handsocket;
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

        private void Start()
        {
            controller = gameObject.GetComponent<CharacterController>();
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
            if (context.phase == InputActionPhase.Canceled)
            {
                //cosespeciali
            }

            if (!IsChargingShot)
                return;

            heldPallo.Throw(transform.forward * 
                (MinThrowForce + (Mathf.Min(currentChargeTime, maxChargeTime)
                * (maxThrowForce - MinThrowForce) / maxChargeTime))
                + Vector3.up * 1.2f);
            
            heldPallo = null;

            currentChargeTime = 0;
        }

        public void OnParry(InputAction.CallbackContext context)
        {

        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            dodging = true;
        }

        //TODO trasformare in unityEvent
        public void KillPlayer()
        {
            Debug.Log("player is killed!");
            //TODO rimuovi destroy e togli una vita
            Destroy(this.gameObject);
        }

        void Update()
        {
            if (!dodging)
			{
                HandleMovement();
                HandleRotation();
			}
			else
                HandleDodge();
        }

        private void HandleMovement()   
        {
            if (playerVelocity.y < 0) //TODO && !stoVenendoSpinto
                playerVelocity.y = 0f;

            controller.Move(movementInput * Time.deltaTime * playerSpeed);
        }

        private void HandleRotation()
        {
            //if was already charging and not charging anymore
            if (currentChargeTime != 0 && rotationInput == Vector3.zero)
            {
                currentChargeTime = 0;
            }
            //if can charge ball
            else if (heldPallo && rotationInput != Vector3.zero)
            {
                //if charge is beginning now
                if (currentChargeTime == 0)
                {
                    //TODO: start animation, change playerspeed
                }
                //charge throw
                currentChargeTime += Time.deltaTime;

                Vector3 rotationVector = transform.position + rotationInput;
                transform.LookAt(rotationVector, Vector3.up);
            }
			else if (movementInput != Vector3.zero)
			{
                Vector3 moveVector = transform.position + movementInput;
                transform.LookAt(moveVector, Vector3.up);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            heldPallo = other.GetComponent<PalloController>();
            if (IsHoldingBall)
            {
                currentChargeTime = 0;
                heldPallo.Hold(handsocket);
            }
        }

        private void HandleDodge()
		{
            dodging = false;
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
    }
}