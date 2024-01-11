using UnityEngine;
using UnityEngine.InputSystem;
using Controllers;

namespace StateMachine
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Rigidbody))]

    public class PlayerController : MonoBehaviour
    {
        private Player player;

		#region properties
		public bool CanMove =>  player.stateMachine.currentState == player.stateMachine.idle    ||
                                player.stateMachine.currentState == player.stateMachine.throww  ||
                                player.stateMachine.currentState == player.stateMachine.dodge   ||
                                player.stateMachine.currentState == player.stateMachine.parry   ||
                                player.stateMachine.currentState == player.stateMachine.stun;

        public bool CanThrow => player.IsHoldingBall &&
                                (player.stateMachine.currentState == player.stateMachine.move);

        public bool CanDodge => player.stateMachine.currentState == player.stateMachine.move;

        public bool CanParry => !player.IsHoldingBall &&
                                (player.stateMachine.currentState == player.stateMachine.move);

        public bool CanStun =>  player.stateMachine.currentState == player.stateMachine.move    ||
                                player.stateMachine.currentState == player.stateMachine.throww  ||
                                player.stateMachine.currentState == player.stateMachine.dodge   ||
                                player.stateMachine.currentState == player.stateMachine.parry;

		#endregion
        
		private void Start()
        {
            player = gameObject.GetComponent<Player>();

            player.currentPlayerSpeed = player.playerSpeed;
        }

        void Update()
        {
            if (player.dodging)
            {
                HandleDodge();
                return;
            }

            HandleRotation();
            HandleThrow();

            if (!player.IsHoldingBall && player.holdBallCooldown > 0)
                player.holdBallCooldown -= Time.deltaTime;
        }

        #region onInput

        public void OnMove(InputAction.CallbackContext context)
        {
            player.movementInput = new Vector3(
                context.ReadValue<Vector2>().x,
                0,
                (context.ReadValue<Vector2>().y));
        }

        public void OnRotation(InputAction.CallbackContext context)
        {
            player.rotationInput = new Vector3(
                context.ReadValue<Vector2>().x,
                0,
                (context.ReadValue<Vector2>().y));
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (CanThrow)
            {
                player.stateMachine.ChangeState(player.stateMachine.throww);
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                player.stateMachine.ChangeState(player.stateMachine.move);
            }
        }

        public void OnParry(InputAction.CallbackContext context)
        {
            if (CanParry)
                player.stateMachine.ChangeState(player.stateMachine.parry);
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
			if (CanDodge)
                player.stateMachine.ChangeState(player.stateMachine.dodge);
        }

        #endregion

        #region HandleInput

        private void HandleRotation()
        {
            //if can charge ball
            if (player.heldPallo && player.rotationInput != Vector3.zero)
            {
                Vector3 rotationVector = transform.position + player.rotationInput;
                transform.LookAt(rotationVector, Vector3.up);
            }
			else if (player.movementInput != Vector3.zero)
			{
                Vector3 moveVector = transform.position + player.movementInput;
                transform.LookAt(moveVector, Vector3.up);
            }
        }

        private void HandleThrow()
		{
            //MARCO aggiungere istruzione per vedere se azione Throw sta venendo premuta sul gamepad
            //c'è istruzione per controllare in update lo stato di un azione
            if (player.IsHoldingBall)
            {
                player.currentChargeTime += Time.deltaTime;
                Debug.Log("currentChargeTime = " + player.currentChargeTime);
            }
        }

        private void HandleDodge()
        {
            //MARCO come fare dodge? salvo direzione + ignoro/disabilito input movimento + controller.Move(savedDirection)?
            player.dodgeCounter += Time.deltaTime;
            if (player.dodgeCounter >= player.dodgeDuration)
                player.dodging = false;
        }

		#endregion

		#region key methods

		private void OnTriggerEnter(Collider other)
        {
            if (player.holdBallCooldown <= 0)
			{
                player.heldPallo = other.GetComponent<PalloController>();
            }
            
            if (player.IsHoldingBall)
                player.heldPallo.Hold(player.handsocket);
        }

        public void TakeDamage(int amount)
        {
            player.CurrentHp -= amount;

            Debug.Log("Player HP = " + player.CurrentHp);

            if (player.CurrentHp <= 0)
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

		#endregion
	}
}