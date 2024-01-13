using System.Threading;
using System.Threading.Tasks;

namespace FancyStateStuff
{
	/// <summary>
	/// State for a <see cref="FancyStateMachine"/>.
	/// </summary>
	public abstract class FancyState
	{
		/// <summary>
		/// StateMachine that this <see cref="FancyState"/> is part of.
		/// <remarks>
		/// Is set through reflection in the <see cref="FancyStateMachine"/> that this is added to.
		/// </remarks>
		/// </summary>
		protected FancyStateMachine FancyStateMachine;

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
		/// Is called when this <see cref="FancyState"/> stops.
		/// </summary>
		/// <param name="fastForwardToken">Cancellation token to fast forward the function.</param>
		public abstract Task Stop(CancellationToken fastForwardToken);
	}
}