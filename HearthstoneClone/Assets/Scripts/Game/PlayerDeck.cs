using System.Collections.Generic;
using System.Linq;
using CardManagement.CardComposition;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace Game
{
	/// <summary>
	/// Deck used by the player in game.
	/// </summary>
	public class PlayerDeck
	{
		/// <summary>
		/// Where a card should be put in the deck.
		/// </summary>
		public enum CardPosition
		{
			Top,
			Random,
			Bottom,
		}
		
		private List<CardInfo> cardsInDeck;

		/// <summary>
		/// Constructs a player deck.
		/// </summary>
		/// <param name="cards">The list of cards that start in the deck.</param>
		public PlayerDeck(IEnumerable<CardInfo> cards)
		{
			cardsInDeck = new List<CardInfo>(cards);
		}

		/// <summary>
		/// Returns the last card in the deck if there is one.
		/// </summary>
		/// <param name="cardInfo">The card.</param>
		/// <param name="position">The position of the card in the deck, defaults to the top of the deck.</param>
		/// <returns>Whether there is a card left.</returns>
		public bool GetCard([CanBeNull] out CardInfo cardInfo, CardPosition position = CardPosition.Top)
		{
			if (cardsInDeck.Count == 0)
			{
				cardInfo = default;
				return false;
			}
			
			switch (position)
			{
				case CardPosition.Random:
					int index = Random.Range(0, cardsInDeck.Count - 1);
					cardInfo = cardsInDeck[index];
					cardsInDeck.RemoveAt(index);
					break;
				case CardPosition.Bottom:
					cardInfo = cardsInDeck[0];
					cardsInDeck.RemoveAt(0);
					break;
				
				case CardPosition.Top:
				default:	
					cardInfo = cardsInDeck[^1];
					cardsInDeck.RemoveAt(cardsInDeck.Count-1);
					break;
			}

			return true;
		}
		
		/// <summary>
		/// Adds a card to the player deck.
		/// </summary>
		/// <param name="cardInfo">The card.</param>
		/// <param name="position">Where to put it in the deck.</param>
		public void AddCard(CardInfo cardInfo, CardPosition position = CardPosition.Random)
		{
			switch (position)
			{
				case CardPosition.Top:
					cardsInDeck.Add(cardInfo);
					break;
				
				case CardPosition.Bottom:
					cardsInDeck = cardsInDeck.Prepend(cardInfo).ToList();
					break;
				
				case CardPosition.Random:
				default:
					cardsInDeck.Insert(Random.Range(0,cardsInDeck.Count-1), cardInfo);
					break;
			}
		}
	}
}