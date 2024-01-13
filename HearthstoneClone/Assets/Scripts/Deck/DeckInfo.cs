using System;
using System.Collections.Generic;
using Deck.DeckManagement;
using JetBrains.Annotations;

namespace Deck
{
	/// <summary>
	/// Contains all of the info necessary to construct a deck.
	/// </summary>
	[Serializable]
	public class DeckInfo : Savable
	{
		private const int MAX_CARD_COUNT = 15;

		/// <summary>
		/// Name of the deck.
		/// </summary>
		[NotNull] public string Name = string.Empty;

		/// <summary>
		/// Description of the deck.
		/// </summary>
		[NotNull] public string Description = string.Empty;
		
		/// <summary>
		/// Paths to the cards in the deck.
		/// </summary>
		[NotNull] public List<string> Cards = new List<string>();

		public DeckInfo() : base(SaveDirectory.Decks){}

		/// <summary>
		/// Adds a card to the list.
		/// <remarks>If the max card amount is reached it will return false.</remarks>
		/// </summary>
		/// <param name="card">Path to the card</param>
		/// <returns>Whether the card can be added.</returns>
		public bool AddCard(string card)
		{
			if (Cards.Count < MAX_CARD_COUNT)
			{
				Cards.Add(card);
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Constructs a string containing debug information about the deck.
		/// </summary>
		/// <returns>A string containing debug information.</returns>
		public override string ToString()
		{
			return $"[Deck( Name: {Name}, Description: {Description}), {base.ToString()}]";
		}
	}
}