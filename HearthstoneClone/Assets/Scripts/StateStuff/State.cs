namespace StateStuff
{
	/// <summary>
	/// State for a <see cref="StateMachine"/>.
	/// </summary>
	public abstract class State
	{
		/// <summary>
		/// StateMachine that this <see cref="State"/> is part of.
		/// <remarks>
		/// Is set through reflection in the <see cref="StateMachine"/> that this is added to.
		/// </remarks>
		/// </summary>
		protected StateMachine StateMachine;

		/// <summary>
		/// Is called when the state starts.
		/// </summary>
		public abstract void Start();
		
		/// <summary>
		/// Is called every frame.
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// Is called when this <see cref="State"/> stops.
		/// </summary>
		public abstract void Stop();
	}
}