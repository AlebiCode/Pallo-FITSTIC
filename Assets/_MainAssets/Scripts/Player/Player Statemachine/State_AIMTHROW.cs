using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_AIMTHROW : PlayerState
	{
		public State_AIMTHROW(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();
		}

		public override void Exit()
		{
			base.Exit();
			ResetCharge();
		}

		public override void Update()
		{
			base.Update();

			player.ThrowChargeCurrent += Time.deltaTime;

			player.HandleMovement(player.MovementDirectionFromInput, player.speedSlow);
			player.HandleRotation();
		}

		private void ResetCharge()
		{
			player.HeldPallo = null;
			player.ThrowChargeCurrent = 0;
			player.HoldBallCooldownCurrent = player.holdBallCooldown;
		}
	}
}

