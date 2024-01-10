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
			owner.currentPlayerSpeed = owner.slowPlayerSpeed;
		}
		public override void Exit()
		{
			base.Enter();

			//throw ball
			owner.heldPallo.Throw(owner.ThrowVelocity);

			//reset variables
			owner.heldPallo = null;
			owner.currentChargeTime = 0;
			owner.currentPlayerSpeed = owner.playerSpeed;
			owner.holdBallCooldown = owner.holdBallCooldownDuration;

			Debug.Log("Throw complete");
		}

		public override void Update()
		{
			base.Update();

		}
	}
}

