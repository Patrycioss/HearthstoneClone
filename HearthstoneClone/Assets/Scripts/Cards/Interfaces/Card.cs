using UnityEngine;


namespace Cards.Interfaces
{
	/// <summary>
	/// A card can be played from the hand. It has a mana cost, a name and a description.
	/// </summary>
	public interface Card
	{
		public int Cost { get; }
		public string Name { get; }	
		public string Description { get; }
	}
}