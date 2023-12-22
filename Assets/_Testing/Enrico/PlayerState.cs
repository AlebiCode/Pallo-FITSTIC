using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public delegate void PlayerEvent(Player owner);
	public abstract class PlayerState : IState
	{
		public event PlayerEvent OnEnter;
		public event PlayerEvent OnExit;

		public Player owner;
		public PlayerState(Player owner)
		{
			this.owner = owner;

		}

		public void Enter()
		{
			OnEnter.Invoke(owner);
		}
		public void Exit()
		{
			OnExit.Invoke(owner);
		}
		public void Update()
		{

		}
	}
}

