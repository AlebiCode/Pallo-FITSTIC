using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_MOVE : PlayerState
	{
		public State_MOVE(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();
			player.PlayerAnimation.PlayAnimation(PlayerAnimation.idle);
		}

		public override void Exit()
		{
			base.Exit();
		}

		public override void Update()
		{
			base.Update();

			player.HandleMovement(player.MovementDirectionFromInput, player.PlayerD.speedRegular);
			player.HandleRotation();
        }
    }
}

