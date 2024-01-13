using System.Threading;
using System.Threading.Tasks;

namespace StateSystem
{
	/// <summary>
	/// State for a <see cref="StateSystem.StateMachine"/>.
	/// </summary>
	public abstract class State
	{
		/// <summary>
		/// StateMachine that this <see cref="State"/> is part of.
		/// <remarks>
		/// Is set through reflection in the <see cref="StateSystem.StateMachine"/> that this is added to.
		/// </remarks>
		/// </summary>
		protected StateMachine StateMachine;

		/// <summary>
		/// Is called when the state starts.
		/// </summary>
		/// <param name="fastForwardToken">Cancellation token to fast forward the function.</param>
		public abstract Task Start(CancellationToken fastForwardToken);
		
		/// <summary>
		/// Is called every frame.
		/// </summary>
		public abstract Task Update();

		/// <summary>
		/// Is called when this <see cref="State"/> stops.
		/// </summary>
		/// <param name="fastForwardToken">Cancellation token to fast forward the function.</param>
		public abstract Task Stop(CancellationToken fastForwardToken);
	}
}