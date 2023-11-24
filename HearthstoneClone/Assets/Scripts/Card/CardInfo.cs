using System;

namespace Card
{
	/// <summary>
	/// Contains all of the info necessary to construct a card.
	/// </summary>
	[Serializable]
	public struct CardInfo
	{
		/// <summary>
		/// Name of the card.
		/// </summary>
		public string Name;
		
		/// <summary>
		/// Mana cost of the card.
		/// </summary>
		public int ManaCost;
		
		/// <summary>
		/// Name of the sprite that the card uses.
		/// </summary>
		public string SpriteName;
	}
}