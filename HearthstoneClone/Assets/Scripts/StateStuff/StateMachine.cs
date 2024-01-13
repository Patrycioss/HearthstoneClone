using System.Reflection;
using UnityEngine;

namespace StateStuff
{
	/// <summary>
	/// Manages states.
	/// </summary>
	public class StateMachine
	{
		/// <summary>
		/// Current active state of the state machine.
		/// </summary>
		public State ActiveState { get; private set; }

		/// <summary>
		/// Makes a new state machine.
		/// </summary>
		/// <param name="startState">The starting state of the state machine.</param>
		public StateMachine(State startState)
		{
			SetState(startState);
		}
		
		/// <summary>
		/// Set a new active state.
		/// </summary>
		/// <param name="state">The new active state.</param>
		public void SetState(State state)
		{
			if (ActiveState != state)
			{ 
				Debug.Log($"Setting state to {state.GetType()}");

				if (ActiveState != null)
				{
					ActiveState.Stop();
					Debug.Log($"Stopped active state: {ActiveState.GetType()}.");
				}

				ActiveState = state;
				
				FieldInfo a = ActiveState.GetType()
					.GetField("stateMachine", BindingFlags.Instance | BindingFlags.NonPublic);

				if (a != null)
				{
					a.SetValue(ActiveState, this);
				}
			
				ActiveState.Start();
			
				Debug.Log($"Started active state: {ActiveState.GetType()}.");
			}
			else
			{
				Debug.Log($"Not setting active state to {state.GetType()} because the state machine is already in that state!");
			}
		}

		/// <summary>
		/// Updates the active states and makes sure the state queue is handled.
		/// Should be called in a Unity Update() function.
		/// </summary>
		public void Update()
		{
			ActiveState?.Update();
		}
	}
}