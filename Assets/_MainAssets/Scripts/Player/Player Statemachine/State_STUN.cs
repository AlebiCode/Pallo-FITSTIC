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
			Debug.Log("Enter Stun pushforce: " + player.PushForce);
			LosePallo();
			player.LookAt(new Vector3(-player.PushDirection.x, 0f, -player.PushDirection.z));
			player.IsInvincibile = true;
		}

		public override void Exit()
		{
			base.Exit();
			player.IsInvincibile = false;
		}

		public override void Update()
		{
			base.Update();
			Debug.Log("pushDirection :" + player.PushDirection + "; pushForce :" + player.PushForce);
			player.HandleMovement(player.PushDirection, player.PushForce);

			player.PushForce -= player.PushDecrease;

			if (player.PushForce <= player.speedSlow)
			{
				player.StateMachine.ChangeState(player.StateMachine.idle);
			}
		}

		private void LosePallo()
		{
			if (player.HeldPallo)
			{
				player.HeldPallo.Drop();
				player.HeldPallo = null;
				player.ThrowChargeCurrent = 0;
				player.HoldBallCooldownCurrent = player.holdBallCooldown;
			}
		}
	}
}

