namespace Card.Interfaces
{
	/// <summary>
	/// A minion can be put on the board if there is room. It can attack and be attacked.
	/// </summary>
	public interface IMinion
	{
		/// <summary>
		/// Attack power.
		/// </summary>
		public int Attack { get; }
		
		/// <summary>
		/// Health points.
		/// </summary>
		public int Health { get; }
	}
}