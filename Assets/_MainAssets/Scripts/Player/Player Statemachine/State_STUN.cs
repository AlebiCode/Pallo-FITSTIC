using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_STUN : PlayerState
	{
		public State_STUN(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();

			player.PlayerAnimation.PlayAnimation(PlayerAnimation.idle);
			LosePallo();
			player.LookAt(new Vector3(-player.PlayerD.PushDirection.x, 0f, -player.PlayerD.PushDirection.z));
			player.PlayerD.IsInvincibile = true;
		}

		public override void Exit()
		{
			base.Exit();
			player.PlayerD.IsInvincibile = false;
		}

		public override void Update()
		{
			base.Update();
			player.HandleMovement(player.PlayerD.PushDirection, player.PlayerD.PushForce);

			player.PlayerD.PushForce -= player.PlayerD.PushDecrease;

			if (player.PlayerD.PushForce <= player.PlayerD.speedSlow)
			{
				player.StateMachine.ChangeState(player.StateMachine.idle);
			}
		}

		private void LosePallo()
		{
			if (player.PlayerD.HeldPallo)
			{
				player.PlayerD.HeldPallo.Drop();
				player.PlayerD.HeldPallo = null;
				player.PlayerD.ThrowChargeCurrent = 0;
				player.PlayerD.HoldBallCooldownCurrent = player.PlayerD.holdBallCooldown;
			}
		}
	}
}

