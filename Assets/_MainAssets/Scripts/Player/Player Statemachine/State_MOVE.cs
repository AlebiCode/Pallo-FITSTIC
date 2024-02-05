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
		}

		public override void Exit()
		{
			base.Exit();
		}

		public override void Update()
		{
			base.Update();

			player.HandleMovement(player.MovementDirectionFromInput, player.speedRegular);
			player.HandleRotation();

			Vector3 leg = player.transform.InverseTransformDirection(player.MovementDirectionFromInput);
            player.PlayerAnimation.LegMovementParameters(leg.x, leg.z);
        }
    }
}

