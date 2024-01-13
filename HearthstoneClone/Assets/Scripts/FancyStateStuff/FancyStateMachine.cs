using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Extensions;
using UnityEngine;

namespace FancyStateStuff
{
	/// <summary>
	/// Manages states.
	/// </summary>
	public class FancyStateMachine
	{
		/// <summary>
		/// Current active state of the state machine.
		/// </summary>
		public FancyState ActiveFancyState { get; private set; }

		// private Task activeSetTask = null;
		// private Task activeStateTask = null;

		private CancellationTokenSource stopTaskFastForwardSource = new CancellationTokenSource();
		private CancellationTokenSource startTaskFastForwardSource = new CancellationTokenSource();

		private FancyState queuedFancyState = null;
		private Task activeSetTask = null;
		private	bool readyForNextState = false;
		
		/// <summary>
		/// Makes a new state machine.
		/// </summary>
		/// <param name="startFancyState">The starting state of the state machine.</param>
		public FancyStateMachine(FancyState startFancyState)
		{
			SetState(startFancyState);
		}
		
		/// <summary>
		/// Set a new active state.
		/// </summary>
		/// <param name="fancyState">The new active state.</param>
		public async void SetState(FancyState fancyState)
		{
			if (queuedFancyState != null)
			{
				FastForwardNextStateStopTask();
				FastForwardNextStateStartTask();

				if (activeSetTask != null)
				{
					await activeSetTask;
				}
			}

			queuedFancyState = fancyState;
			
			HandleQueue();
		}

		private void HandleQueue()
		{
			if (activeSetTask is null or {IsCanceled: true} or {IsCompleted: true} or {IsFaulted: true})
			{
				if (activeSetTask != null)
				{
					activeSetTask?.Dispose();
					activeSetTask = null;
				}
				readyForNextState = true;
			}
			
			if (readyForNextState && queuedFancyState != null)
			{
				activeSetTask = InternalSetState(queuedFancyState);
				queuedFancyState = null;
			}
		}

		private async Task InternalSetState(FancyState fancyState)
		{
			if (ActiveFancyState != fancyState)
			{ 
				Debug.Log($"Setting state to {fancyState.GetType()}");

				if (ActiveFancyState != null)
				{
					await StopCurrent();
				}

				await StartNew(fancyState);
			}
			else
			{
				Debug.Log($"Not setting active state to {fancyState.GetType()} because the state machine is already in that state!");
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
			HandleQueue();

			if (ActiveFancyState != null)
			{
				await ActiveFancyState.Update();
			}
		}

		private async Task StopCurrent()
		{
			await ActiveFancyState.Stop(stopTaskFastForwardSource.Token);

			
			Debug.Log($"Stopped active state: {ActiveFancyState.GetType()}.");
		}

		private async Task StartNew(FancyState fancyState)
		{
			ActiveFancyState = fancyState;
				
			FieldInfo a = ActiveFancyState.GetType()
				.GetField("stateMachine", BindingFlags.Instance | BindingFlags.NonPublic);

			if (a != null)
			{
				a.SetValue(ActiveFancyState, this);
			}
			
			await ActiveFancyState.Start(startTaskFastForwardSource.Token);
			
			Debug.Log($"Started active state: {ActiveFancyState.GetType()}.");
		}
	}
}