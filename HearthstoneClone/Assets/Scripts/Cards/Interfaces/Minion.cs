namespace Cards.Interfaces
{
	/// <summary>
	/// A minion can be put on the board if there is room. It can attack and be attacked.
	/// </summary>
	public interface Minion
	{
		public int Attack { get; }
		public int Health { get; }
	}
}