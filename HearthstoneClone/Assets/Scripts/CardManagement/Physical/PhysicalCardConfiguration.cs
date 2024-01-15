using CardManagement.CardComposition;
using Game;
using UnityEngine;

namespace CardManagement.Physical
{
	/// <summary>
	/// Configuration used to initialize a <see cref="PhysicalCard"/>.
	/// </summary>
	public struct PhysicalCardConfiguration
	{
		/// <summary>
		/// Info associated with the card.
		/// </summary>
		public CardInfo CardInfo;

		/// <summary>
		/// The player to which this card belongs.
		/// </summary>
		public Player Player;

		/// <summary>
		/// Intermediate container to store cards in in the hierarchy.
		/// </summary>
		public Transform IntermediateContainer;
		
		/// <summary>
		/// Stores the cards on the board.
		/// </summary>
		public Board Board;
	}
}