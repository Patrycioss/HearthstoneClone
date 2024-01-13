using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Extensions;
using UnityEngine;

namespace StateSystem
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

		// private Task activeSetTask = null;
		// private Task activeStateTask = null;

		private CancellationTokenSource stopTaskFastForwardSource = new CancellationTokenSource();
		private CancellationTokenSource startTaskFastForwardSource = new CancellationTokenSource();
		
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
		public async void SetState(State state)
		{
			if (ActiveState != state)
			{ 
				Debug.Log($"Setting state to {state.GetType()}");

				if (ActiveState != null)
				{
					await StopCurrent();
				}

				await StartNew(state);
			}
			else
			{
				Debug.Log($"Not setting active state to {state.GetType()} because the state machine is already in that state!");
			}
			
			startTaskFastForwardSource = startTaskFastForwardSource.Reset();
			stopTaskFastForwardSource = stopTaskFastForwardSource.Reset();
		}

		/// <summary>
		/// Causes the next state stop task to get fast-forwarded.
		/// </summary>
		public void FastForwardNextStateStopTask()
		{
			Debug.Log($"Fast forwarding stop task!");
			if (!stopTaskFastForwardSource.IsCancellationRequested)
			{
				stopTaskFastForwardSource.Cancel();
			}
		}
		
		/// <summary>
		/// Causes the next state start task to get fast-forwarded.
		/// </summary>
		public void FastForwardNextStateStartTask()
		{
			Debug.Log($"Fast forwarding start task!");
			if (!startTaskFastForwardSource.IsCancellationRequested)
			{
				startTaskFastForwardSource.Cancel();
			}
		}

		/// <summary>
		/// Updates the active states and makes sure the state queue is handled.
		/// Should be called in a Unity Update() function.
		/// </summary>
		public async Task Update()
		{
			// if (activeSetTask is null or {IsCanceled: true} or {IsCompleted: true} or {IsFaulted: true})
			// {
			// 	activeSetTask?.Dispose();
			// 	readyForNextState = true;
			// }
			// else readyForNextState = false;
			//
			// if (readyForNextState && queuedState != null)
			// {
			// 	readyForNextState = false;
			// 	activeSetTask = SetState(queuedState);
			// 	queuedState = default;
			// }

			if (ActiveState != null)
			{
				await ActiveState.Update();
			}
		}

		private async Task StopCurrent()
		{
			await ActiveState.Stop(stopTaskFastForwardSource.Token);

			
			Debug.Log($"Stopped active state: {ActiveState.GetType()}.");
		}

		private async Task StartNew(State state)
		{
			ActiveState = state;
				
			FieldInfo a = ActiveState.GetType()
				.GetField("stateMachine", BindingFlags.Instance | BindingFlags.NonPublic);

			if (a != null)
			{
				a.SetValue(ActiveState, this);
			}
			
			await ActiveState.Start(startTaskFastForwardSource.Token);
			
			Debug.Log($"Started active state: {ActiveState.GetType()}.");
		}
	}
}