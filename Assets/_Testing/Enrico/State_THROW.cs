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
			player.currentPlayerSpeed = player.slowPlayerSpeed;
		}
		public override void Exit()
		{
			base.Exit();
			//throw ball
			player.heldPallo.Throw(player.ThrowVelocity);

			//reset variables
			player.heldPallo = null;
			player.currentChargeTime = 0;
			player.currentPlayerSpeed = player.playerSpeed;
			player.holdBallCooldown = player.holdBallCooldownDuration;

			Debug.Log("Throw complete");
		}

		public override void Update()
		{
			base.Update();
			player.currentChargeTime += Time.deltaTime;
		}
	}
}

