using System.Collections.Generic;
using System.Reflection;
using Extensions;
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
		
		private Dictionary<string, object> referenceStorage = new Dictionary<string, object>();
		
		/// <summary>
		/// Set a new active state.
		/// </summary>
		/// <param name="state">The new active state.</param>
		public void SetState(State state)
		{
			if (ActiveState == null || ActiveState.GetType() != state.GetType())
			{ 
				TimedLogger.Log($"Setting state to {state.GetType()}");

				if (ActiveState != null)
				{
					ActiveState.Stop();
					Debug.Log($"Stopped active state: {ActiveState.GetType()}.");
				}

				ActiveState = state;
				
				FieldInfo a = ActiveState.GetType()
					.GetField("StateMachine", BindingFlags.Instance | BindingFlags.NonPublic);

				if (a != null)
				{
					a.SetValue(ActiveState, this);
				}
			
				ActiveState.Start();
			
				TimedLogger.Log($"Started active state: {ActiveState.GetType()}.");
			}
			else
			{
				TimedLogger.Log($"Not setting active state to {state.GetType()} because the state machine is already in that state!");
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

		/// <summary>
		/// Add an object to store.
		/// </summary>
		/// <param name="name">Name of the reference.</param>
		/// <param name="reference">Object to store.</param>
		public void AddReference<T>(string name, T reference)
		{
			referenceStorage.TryAdd(name, reference);
		}

		/// <summary>
		/// Get a reference from storage.
		/// </summary>
		/// <param name="name">Name of the reference.</param>
		/// <param name="reference">The referenced object.</param>
		/// <returns></returns>
		public void GetReference<T>(string name, out T reference)
		{
			if (referenceStorage.TryGetValue(name, out object obj))
			{
				reference = (T) obj;
				return;
			}

			reference = default;
		}
	}
}