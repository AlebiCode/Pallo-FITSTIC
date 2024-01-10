using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class State_DODGE : PlayerState
	{
		public State_DODGE(Player owner) : base(owner)
		{

		}

		public override void Enter()
		{
			base.Enter();
			owner.dodging = true;
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
