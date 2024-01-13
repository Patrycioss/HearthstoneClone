using System;
using System.Collections.Generic;
using CardManagement.CardComposition;
using Deck.DeckManagement;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Deck
{
	/// <summary>
	/// Contains all of the info necessary to construct a deck.
	/// </summary>
	[Serializable]
	public class DeckInfo : Savable
	{
		private static readonly int MaxCardCount = 15;
		
		/// <summary>
		/// Name of the deck.
		/// </summary>
		[NotNull] public string Name = string.Empty;

		/// <summary>
		/// Description of the deck.
		/// </summary>
		[NotNull] public string Description = string.Empty;

		/// <summary>
		/// Image for the thumbnail of the deck.
		/// </summary>
		[CanBeNull] public AssetReference Image = null;

		/// <summary>
		/// Cards in the deck.
		/// </summary>
		[NotNull] public List<string> Cards = new List<string>();

		public DeckInfo() : base(SaveDirectory.Decks)
		{
			
		}

		/// <summary>
		/// Adds a card to the list.
		/// <remarks>If the max card amount is reached it will return false.</remarks>
		/// </summary>
		/// <param name="card">Path to the card</param>
		/// <returns>Whether the card can be added.</returns>
		public bool AddCard(string card)
		{
			if (Cards.Count < MaxCardCount)
			{
				Cards.Add(card);
				return true;
			}

			return false;
		}
		
		public override string ToString()
		{
			return $"[Deck( Name: {Name}, Description: {Description}), {base.ToString()}]";
		}
	}
}