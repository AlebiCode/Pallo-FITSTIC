using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public interface IState
	{
		public void Enter();
		public void Exit();
		public void Update();
	}
}

