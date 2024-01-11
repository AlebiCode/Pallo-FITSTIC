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

			HandleMovement();
			HandleRotation();
		}

		private void HandleMovement()
		{
			if (owner.playerVelocity.y < 0)
				owner.playerVelocity.y = 0f;

			owner.controller.Move(owner.movementInput * Time.deltaTime * owner.currentPlayerSpeed);
		}

		private void HandleRotation()
		{
			if (owner.heldPallo && owner.rotationInput != Vector3.zero)
			{
				Vector3 rotationVector = owner.transform.position + owner.rotationInput;
				owner.transform.LookAt(rotationVector, Vector3.up);
			}
			else if (owner.movementInput != Vector3.zero)
			{
				Vector3 moveVector = owner.transform.position + owner.movementInput;
				owner.transform.LookAt(moveVector, Vector3.up);
			}
		}
	}
}

