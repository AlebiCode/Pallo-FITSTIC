using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
	public class StateMachine
	{
		public Player player;
		public PlayerState currentState;

		public State_PARRY parry;
		public StateMachine()
		{
			parry = new State_PARRY(player);
			parry.OnExit += PrintBestemmia;


		}

		public void DiventaCristiano()
		{
			parry.OnExit -= PrintBestemmia;
		}

		public void ChangeState(PlayerState state)
		{
			currentState.Exit();

		}

		void PrintBestemmia(Player owner)
		{
			Debug.Log("Accipigna");
		}
	}
}

