using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_MOVE : PlayerState
	{
		public Vector3 moveVector;
		private float rotateSmoothing = 3f;

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
			player.controller.Move(player.MovementDirectionFromInput * Time.deltaTime * player.playerSpeed);
		}

		private void HandleRotation()
		{
			if (player.usingGamePad)
			{
				GamepadRotation();
			}
			else
			{
				KeyboardRotation();
			}
			/*if (player.rotationInput != Vector3.zero)
			{
				moveVector = player.transform.position + player.rotationInput;
				player.transform.LookAt(moveVector, Vector3.up);
			}

			else if (player.movementInput != Vector3.zero)
			{
				moveVector = player.transform.position + player.movementInput;
				player.transform.LookAt(moveVector, Vector3.up);
			}
			*/
		}

		private void GamepadRotation()
		{
			if (Mathf.Abs(player.rotationInput.x) > 0 || Mathf.Abs(player.rotationInput.y) > 0) // forse > controllerDeadzone invece di > 0?
			{
				Vector3 playerDirection = Vector3.right * player.rotationInput.x + Vector3.forward * player.rotationInput.y;
				if (playerDirection.sqrMagnitude > 0.0f)
				{
					Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
					player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, rotateSmoothing * Time.deltaTime);
				}
			}
			else if (player.movementInput != Vector2.zero)
			{
				moveVector = player.transform.position + player.MovementDirectionFromInput;
				player.transform.LookAt(moveVector, Vector3.up);
			}
		}

		private void KeyboardRotation()
		{
			Ray cameraRay = player.mainCamera.ScreenPointToRay(player.rotationInput);
			Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
			float rayLength;

			if (groundPlane.Raycast(cameraRay, out rayLength))
			{
				Vector3 pointToLook = cameraRay.GetPoint(rayLength);
				Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);
				player.LookAt(pointToLook);
			}
		}
	}
}

