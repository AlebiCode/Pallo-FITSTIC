using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_DEATH : PlayerState
	{
		public State_DEATH(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();
			player.PlayerAnimation.PlayAnimation(PlayerAnimation.death);
		}

		public override void Exit()
		{
			base.Exit();
		}

		public override void Update()
		{
			base.Update();
		}
	}
}

