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
        }

        void Update()
        {
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

            if (context.phase == InputActionPhase.Canceled && player.stateMachine.currentState == player.stateMachine.throww)
            {
                player.heldPallo.Throw(player.ThrowVelocity);
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
	}
}