using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_DODGE : PlayerState
	{
		//private Vector3 dodgeDirection = Vector3.zero;

		public State_DODGE(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();
			//dodgeDirection = owner.rotationInput;
			player.dodging = true;
		}

		public override void Exit()
		{
			base.Exit();

			player.dodging = false;
		}

		public override void Update()
		{
			base.Update();

			player.controller.Move(player.movementInput * Time.deltaTime * player.currentPlayerSpeed);

			player.dodgeCounter += Time.deltaTime;
			if (player.dodgeCounter >= player.dodgeDuration)
				player.stateMachine.ChangeState(player.stateMachine.move);

		}
	}

}
