using UnityEngine;
using UnityEngine.InputSystem;
using Controllers;

namespace StateMachine
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterController))]

    public class PlayerController : MonoBehaviour
    {
        private Player player;

		#region properties
		public bool CanMove =>  player.stateMachine.currentState == player.stateMachine.idle;

        public bool CanThrow => player.IsHoldingBall &&
                                (player.stateMachine.currentState == player.stateMachine.move);

        public bool CanDodge => player.stateMachine.currentState == player.stateMachine.move;

        public bool CanParry => !player.IsHoldingBall &&
                                (player.stateMachine.currentState == player.stateMachine.move);

        public bool CanStun =>  player.stateMachine.currentState == player.stateMachine.move    ||
                                player.stateMachine.currentState == player.stateMachine.aimthrow  ||
                                player.stateMachine.currentState == player.stateMachine.dodge   ||
                                player.stateMachine.currentState == player.stateMachine.parry;

		#endregion
        
		private void Start()
        {
            player = gameObject.GetComponent<Player>();
        }

        void Update()
        {
            if (!player.IsHoldingBall && player.holdBallCooldown > 0)
                player.holdBallCooldown -= Time.deltaTime;
        }

        #region onInput

        public void OnMove(InputAction.CallbackContext context)
        {
            player.movementInput = context.ReadValue<Vector2>();

            if (CanMove)
                player.stateMachine.ChangeState(player.stateMachine.move);

            if (context.phase == InputActionPhase.Canceled && player.stateMachine.currentState == player.stateMachine.move)
			{
                player.stateMachine.ChangeState(player.stateMachine.idle);
            }
        }

        public void OnRotation(InputAction.CallbackContext context)
        {
            player.rotationInput = context.ReadValue<Vector2>();
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (CanThrow)
                player.stateMachine.ChangeState(player.stateMachine.aimthrow);

            if (context.phase == InputActionPhase.Canceled && player.stateMachine.currentState == player.stateMachine.aimthrow)
            {
                player.heldPallo.Throw(player.ThrowVelocity);
                player.stateMachine.ChangeState(player.stateMachine.idle);
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
	}
}