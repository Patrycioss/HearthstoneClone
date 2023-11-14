using System.Reflection;
using UnityEngine;

namespace StateSystem
{
	/// <summary>
	/// Manages states.
	/// </summary>
	public class StateMachine
	{
		public State CurrentState { get; private set; }
		public State QueuedState { get; private set; }
		
		/// <summary>
		/// Stop the <see cref="CurrentState"/> and start a new <see cref="State"/>
		/// </summary>
		/// <param name="state"></param>
		public void SetState(State state)
		{
			QueuedState = state;
			
			if (CurrentState == QueuedState)
			{
				QueuedState = null;
				return;
			}
			
			if (CurrentState == null)
			{
				StartQueuedState();
				return;
			}

			CurrentState?.Stop(() =>
			{
				Debug.Log($"State: {CurrentState?.GetType()} stopped.");
				StartQueuedState();
			});
		}
		
		/// <summary>
		/// Updates the active states. Should be called in a Unity Update() function.
		/// </summary>
		public void Update()
		{
			CurrentState?.Update();
		}

		/// <summary>
		/// Starts the queued state.
		/// </summary>
		private void StartQueuedState()
		{
			CurrentState = QueuedState;
			QueuedState = null;
			
			var a = CurrentState.GetType()
				.GetField("stateMachine", BindingFlags.Instance | BindingFlags.NonPublic);

			if (a != null)
			{
				a.SetValue(CurrentState, this);
			}
			
			CurrentState.Start();
			Debug.Log($"State: {CurrentState.GetType()} started.");
		}
	}
}