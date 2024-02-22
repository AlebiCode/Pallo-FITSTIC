using UnityEngine;
using UnityEngine.InputSystem;
using Controllers;

namespace StateMachine
{
    public class PlayerController : MonoBehaviour
    {
        private Player player;

		#region properties
		public bool CanMove =>  player.StateMachine.currentState == player.StateMachine.idle;

        public bool CanThrow => player.IsHoldingBall                                                &&
                                (player.StateMachine.currentState == player.StateMachine.idle       ||
                                 player.StateMachine.currentState == player.StateMachine.move);

        public bool CanDodge => player.DodgeCooldownCurrent <= 0                                    &&
                                (player.StateMachine.currentState == player.StateMachine.idle       ||
                                 player.StateMachine.currentState == player.StateMachine.move);

        public bool CanParry => player.DodgeCooldownCurrent <= 0                                    &&
                                !player.IsHoldingBall                                               &&
                                (player.StateMachine.currentState == player.StateMachine.idle       ||
                                 player.StateMachine.currentState == player.StateMachine.move);

        public bool CanStun =>  player.StateMachine.currentState == player.StateMachine.idle        ||
                                player.StateMachine.currentState == player.StateMachine.move        ||
                                player.StateMachine.currentState == player.StateMachine.aimthrow    ||
                                player.StateMachine.currentState == player.StateMachine.dodge       ||
                                player.StateMachine.currentState == player.StateMachine.parry;

        #endregion

        private void Start()
        {
            player = gameObject.GetComponent<Player>();
        }

        #region onInput

        public void OnMove(InputAction.CallbackContext context)
        {
            player.MovementInput = context.ReadValue<Vector2>();

            if (CanMove)
                player.StateMachine.ChangeState(player.StateMachine.move);

            if (context.phase == InputActionPhase.Canceled && player.StateMachine.currentState == player.StateMachine.move)
			{
                player.StateMachine.ChangeState(player.StateMachine.idle);
            }
        }

        public void OnRotation(InputAction.CallbackContext context)
        {
            player.RotationInput = context.ReadValue<Vector2>();
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (CanThrow)
                player.StateMachine.ChangeState(player.StateMachine.aimthrow);

            if (context.phase == InputActionPhase.Canceled && player.StateMachine.currentState == player.StateMachine.aimthrow)
            {
                player.PlayerAnimation.PlayAnimation(PlayerAnimation.throwRightOutro);
                player.HeldPallo.Throw(player.ThrowVelocity);
                player.StateMachine.ChangeState(player.StateMachine.idle);
            }
        }

        public void OnParry(InputAction.CallbackContext context)
        {
            if (CanParry)
                player.StateMachine.ChangeState(player.StateMachine.parry);
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
			if (CanDodge)
                player.StateMachine.ChangeState(player.StateMachine.dodge);
        }

        #endregion
	}
}