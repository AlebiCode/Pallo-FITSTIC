using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Rigidbody))]

    public class PlayerController : MonoBehaviour
    {
        private CharacterController controller;
        private Vector3 playerVelocity;
        private bool groundedPlayer;
        [SerializeField] private float playerSpeed = 1f; //tradotto
        private Vector2 movementInput = Vector2.zero;
        private Vector3 mousePosition;
        private bool dodged = false;
        private PalloController heldPallo;

        private void Start()
        {
            controller = gameObject.GetComponent<CharacterController>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        public void OnRotation(InputAction.CallbackContext context)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                mousePosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
		{

		}

        public void OnParry(InputAction.CallbackContext context)
        {

        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            dodged = context.action.triggered;
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
            UpdateMovement();

            UpdateRotation();

            UpdateThrow();

            UpdateParry();

            UpdateDodge();
        }

        private void UpdateMovement()
        {
            //groundedPlayer = controller.isGrounded; //TODO forse utile per quando viene spinto fuori mappa
            if (playerVelocity.y < 0 /*TODO && !stoVenendoSpinto*/)
                playerVelocity.y = 0f;

            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            if (transform.position.y < 2)
                controller.Move(move * Time.deltaTime * playerSpeed);
        }

        private void UpdateRotation()
        {
            //TODO gestire anche da gamepad (if keyboard -> mouseposition?)
            transform.LookAt(mousePosition, Vector3.up);
        }

        private void UpdateThrow()
        {
            
        }
        private void UpdateParry()
        {
            
        }
        private void UpdateDodge()
        {
            
        }
    }
}