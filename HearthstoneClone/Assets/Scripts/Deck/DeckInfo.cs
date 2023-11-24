using System;
using System.Collections.Generic;
using Card;
using UnityEngine;

namespace Deck
{
	/// <summary>
	/// Contains all of the info necessary to construct a deck.
	/// </summary>
	[Serializable]
	public class DeckInfo
	{
		/// <summary>
		/// Name of the deck.
		/// </summary>
		public string Name = null;

		/// <summary>
		/// Description of the deck.
		/// </summary>
		public string Description = null;

		/// <summary>
		/// Image for the thumbnail of the deck.
		/// </summary>
		public Sprite Image = null;
		
		/// <summary>
		/// Cards in the deck.
		/// </summary>
		public List<CardInfo> Cards = null;

		public override string ToString()
		{
			return $"{Name}/{Description}";
		}
	}
}