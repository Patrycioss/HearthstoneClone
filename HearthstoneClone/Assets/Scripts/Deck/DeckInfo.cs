using System;
using System.Collections.Generic;
using Card;

namespace Deck
{
	/// <summary>
	/// Info of what cards are in the deck, can be used to pull cards.
	/// </summary>
	[Serializable]
	public struct DeckInfo
	{
		public List<CardInfo> cards;

		/// <summary>
		/// Pull a random card from the deck.
		/// </summary>
		public CardInfo PullRandomCard()
		{
			int index = UnityEngine.Random.Range(0, cards.Count);
			CardInfo cardInfo = cards[index];
			cards.RemoveAt(index);
			return cardInfo;
		}
	}
}