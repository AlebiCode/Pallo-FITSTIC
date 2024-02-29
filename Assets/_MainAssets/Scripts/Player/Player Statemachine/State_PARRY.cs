using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace StateMachine
{
	public class State_PARRY : PlayerState
	{
		RaycastHit hit;
		PalloController hitPallo;
		private float parryFrames;
		private float parryDurationCurrent;

		public State_PARRY(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();
			
			parryDurationCurrent = player.PlayerD.parryDuration;
			parryFrames = parryDurationCurrent / 100f * player.PlayerD.parryPercentage;
			player.PlayerD.IsInvincibile = true;
			player.PlayerAnimation.PlayAnimation(PlayerAnimation.takeIntro);
		}

		public override void Exit()
		{
			base.Exit();
			player.PlayerD.ParryCooldownCurrent = player.PlayerD.parryCooldown;
		}

		public override void Update()
		{
			base.Update();

			if (parryDurationCurrent <= 0)
			{
                player.StateMachine.ChangeState(player.StateMachine.idle);
			}
			else if (parryDurationCurrent <= parryFrames)
			{
                player.PlayerAnimation.PlayAnimation(PlayerAnimation.takeOutroNoBall);
				player.PlayerD.IsInvincibile = false;
			}
			else
			{
				Parry();
			}
			
			parryDurationCurrent -= Time.deltaTime;
		}

		public void Parry()
		{
			Physics.SphereCast(player.transform.position, player.PlayerD.parryRange, Vector3.zero, out hit);
			if (!hit.collider)
				return;
			hitPallo = hit.collider.gameObject.GetComponent<PalloController>();
			if (!hitPallo)
				return;


			player.PlayerD.HeldPallo = hitPallo;
			player.PlayerD.HeldPallo.Hold(player.PlayerD.Handsocket);
			//TODO: parry VFX
			player.PlayerAnimation.PlayAnimation(PlayerAnimation.takeOutroBall);

			player.StateMachine.ChangeState(player.StateMachine.idle);
		}
	}
}


