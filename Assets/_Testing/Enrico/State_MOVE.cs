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

			if (owner.playerVelocity.y < 0) //TODO && !stoVenendoSpinto
				owner.playerVelocity.y = 0f;

			owner.controller.Move(owner.movementInput * Time.deltaTime * owner.currentPlayerSpeed);
		}
	}
}

