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

			//reset variables
			player.heldPallo = null;
			player.currentChargeTime = 0;
			player.holdBallCooldown = player.holdBallCooldownDuration;
		}

		public override void Update()
		{
			base.Update();

			player.currentChargeTime += Time.deltaTime;

			player.HandleMovement(player.MovementDirectionFromInput, player.slowSpeed);
			player.HandleRotation();
		}
	}
}

