using UnityEngine;

namespace Card
{
	/// <summary>
	/// Contains all of the info necessary to construct a card.
	/// </summary>
	public struct CardInfo
	{
		/// <summary>
		/// Name of the card.
		/// </summary>
		public string Name;
		
		/// <summary>
		/// Mana cost of the card.
		/// </summary>
		public string ManaCost;
		
		/// <summary>
		/// Image for the card.
		/// </summary>
		public Sprite Image;
	}
}