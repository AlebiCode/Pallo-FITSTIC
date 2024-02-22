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

			if (player.MovementInput != Vector2.zero)
				player.StateMachine.ChangeState(player.StateMachine.move);
			else
				player.PlayerAnimation.PlayAnimation(PlayerAnimation.idle);
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

