using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_THROW : PlayerState
	{
		public State_THROW(Player owner) : base(owner)
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

			Debug.Log("Throw complete");
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

