using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_DODGE : PlayerState
	{
		public Vector3 dodgeDirection;
		public float dodgeCurrentSpeed = 0f;
		public float dodgeCurrentTime = 0f;

		public State_DODGE(Player owner) : base(owner)
		{
			dodgeCurrentSpeed = player.dodgePlayerSpeed;
		}

		public override void Enter()
		{
			base.Enter();
			dodgeDirection = player.MovementDirectionFromInput;
			dodgeCurrentSpeed = player.dodgePlayerSpeed;
			dodgeCurrentTime = player.dodgeMaxDuration;
		}

		public override void Exit()
		{
			base.Exit();
			player.dodgeCurrentCooldown = player.dodgeCooldownTimer;
		}

		public override void Update()
		{
			base.Update();

			Dodge();

			if (dodgeCurrentTime <= 0)
				player.stateMachine.ChangeState(player.stateMachine.move);
		}

		private void Dodge()
		{
			player.controller.Move(dodgeDirection * Time.deltaTime * dodgeCurrentSpeed);

			dodgeCurrentTime -= 2 * Time.deltaTime;
			dodgeCurrentSpeed -= 2 * Time.deltaTime;
		}
	}

}
