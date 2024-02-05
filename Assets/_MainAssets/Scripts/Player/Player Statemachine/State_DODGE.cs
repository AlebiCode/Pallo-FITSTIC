using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_DODGE : PlayerState
	{
		public Vector3 dodgeDirection;
		public float dodgeSpeedCurrent = 0f;
		public float dodgeDurationCurrent = 0f;

		public State_DODGE(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();
			dodgeDirection = player.transform.forward;
			player.transform.LookAt(player.transform.position + dodgeDirection, Vector3.up);
			dodgeSpeedCurrent = player.speedDodge;
			dodgeDurationCurrent = player.dodgeDuration;
			player.PlayerAnimation.PlayAnimation(PlayerAnimation.dashForwardIntro);
		}

		public override void Exit()
		{
			base.Exit();
			player.DodgeCooldownCurrent = player.dodgeCooldown;
		}

		public override void Update()
		{
			base.Update();

			Dodge();

			dodgeDurationCurrent -= Time.deltaTime;

			if (dodgeDurationCurrent <= 0)
			{
				player.StateMachine.ChangeState(player.StateMachine.idle);
                player.PlayerAnimation.PlayAnimation(PlayerAnimation.dashForwardOutro);
            }
        }

		private void Dodge()
		{
			player.HandleMovement(dodgeDirection, dodgeSpeedCurrent);
			dodgeSpeedCurrent -= player.dodgeDecrease;
			Debug.Log("Dodge Current Speed: " + dodgeSpeedCurrent);
		}
	}
}
