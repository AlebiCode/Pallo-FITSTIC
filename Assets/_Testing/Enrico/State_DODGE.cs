using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_DODGE : PlayerState
	{
		public State_DODGE(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();
			player.dodgeDirection = player.movementInput;
			player.dodgeCurrentTime = player.dodgeMaxDuration;
			player.dodgeCurrentSpeed = player.dodgePlayerSpeed;
		}

		public override void Exit()
		{
			base.Exit();
		}

		public override void Update()
		{
			base.Update();

			Dodge();

			if (player.dodgeCurrentTime <= 0)
				player.stateMachine.ChangeState(player.stateMachine.move);
		}

		private void Dodge()
		{
			player.controller.Move(player.dodgeDirection * Time.deltaTime * player.dodgeCurrentSpeed);

			player.dodgeCurrentTime -= Time.deltaTime;
			player.dodgeCurrentSpeed -= Time.deltaTime;
		}
	}

}
