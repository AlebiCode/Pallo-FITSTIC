using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public delegate void PlayerEvent(Player player);
	public abstract class PlayerState : IState
	{
		public event PlayerEvent OnEnter;
		public event PlayerEvent OnExit;

		public Player player;
		public PlayerState(Player player)
		{
			this.player = player;

		}

		public virtual void Enter()
		{
			OnEnter?.Invoke(player);
		}

		public virtual void Exit()
		{
			OnExit?.Invoke(player);
		}

		public virtual void Update()
		{

		}
	}
}

