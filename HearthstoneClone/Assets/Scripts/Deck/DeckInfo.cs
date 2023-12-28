using System;
using System.Collections.Generic;
using CardManagement.CardComposition;
using Deck.DeckManagement;
using JetBrains.Annotations;

namespace Deck
{
	/// <summary>
	/// Contains all of the info necessary to construct a deck.
	/// </summary>
	[Serializable]
	public class DeckInfo : Savable, ITransferable
	{
		/// <summary>
		/// Name of the deck.
		/// </summary>
		[NotNull] public string Name = string.Empty;

		/// <summary>
		/// Description of the deck.
		/// </summary>
		[NotNull] public string Description = string.Empty;

		// Todo: Get better image solution.
		// /// <summary>
		// /// Image for the thumbnail of the deck.
		// /// </summary>
		// [CanBeNull] public Sprite Image = null;

		/// <summary>
		/// Cards in the deck.
		/// </summary>
		[NotNull] public List<CardInfo> Cards = new List<CardInfo>();

		public DeckInfo() : base(SaveDirectory.Decks)
		{
			
		}
		
		public override string ToString()
		{
			return $"[Deck( Name: {Name}, Description: {Description}), {base.ToString()}]";
		}
	}
}