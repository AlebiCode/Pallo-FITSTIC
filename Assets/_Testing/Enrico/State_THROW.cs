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
		}

		public override void Exit()
		{
			base.Exit();

			//reset variables
			player.heldPallo = null;
			player.currentChargeTime = 0;
			player.holdBallCooldown = player.holdBallCooldownDuration;

			Debug.Log("Throw complete");
		}

		public override void Update()
		{
			base.Update();

			player.currentChargeTime += Time.deltaTime;

			HandleMovement();
			HandleRotation();
		}

		private void HandleMovement()
		{
			player.controller.Move(player.movementInput * Time.deltaTime * player.slowPlayerSpeed);
		}

		private void HandleRotation()
		{
			if (player.RotationDirectionFromInput != Vector3.zero)
			{
				Vector3 rotationVector = player.transform.position + player.RotationDirectionFromInput;
				player.transform.LookAt(rotationVector, Vector3.up);
			}
			else if (player.MovementDirectionFromInput != Vector3.zero)
			{
				Vector3 moveVector = player.transform.position + player.MovementDirectionFromInput;
				player.transform.LookAt(moveVector, Vector3.up);
			}
		}
	}
}

