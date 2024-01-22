using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_IDLE : PlayerState
	{
		public State_IDLE(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();

			if (player.movementInput != Vector2.zero)
				player.stateMachine.ChangeState(player.stateMachine.move);
		}

		public override void Exit()
		{
			base.Exit();
		}

		public override void Update()
		{
			base.Update();

			player.HandleRotation();
		}
	}
}

