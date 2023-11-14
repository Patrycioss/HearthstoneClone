using System;
using System.Collections.Generic;
using Card;
using UnityEngine;

namespace Deck
{
	/// <summary>
	/// Info of a created deck.
	/// </summary>
	[Serializable]
	public struct DeckInfo
	{
		/// <summary>
		/// Name of the deck.
		/// </summary>
		public string Name;

		/// <summary>
		/// Description of the deck.
		/// </summary>
		public string Description;

		/// <summary>
		/// Image for the thumbnail of the deck.
		/// </summary>
		public Sprite Image;
		
		/// <summary>
		/// Cards in the deck.
		/// </summary>
		public List<CardInfo> cards;
	}
}