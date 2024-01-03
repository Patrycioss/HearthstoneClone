using System.Collections.Generic;
using System.Linq;
using CardManagement.CardComposition;
using JetBrains.Annotations;
using Utils;
using Random = UnityEngine.Random;

namespace Game
{
	/// <summary>
	/// Object containing all the information of a character in the game.
	/// </summary>
	public class Player
	{
		
		
		/// <summary>
		/// Health of the player.
		/// </summary>
		public ResourceContainer Health { get; }
		
		/// <summary>
		/// Deck that the player uses.
		/// </summary>
		public PlayerDeck PlayerDeck { get; }


		/// <summary>
		/// Construct a new player with a set of cards in the deck.
		/// </summary>
		/// <param name="cards"></param>
		public Player(IEnumerable<CardInfo> cards)
		{
			Health = new ResourceContainer(GameManager.Instance.PlayerStartHealth);
			PlayerDeck = new PlayerDeck(CollectionUtils.RandomizeList(cards.ToList()));
		}

		/// <summary>
		/// Returns the last card in the deck if there is one.
		/// </summary>
		/// <param name="cardInfo">The card.</param>
		/// <returns>Whether there is a card left.</returns>
		public bool GetCard([CanBeNull] out CardInfo cardInfo)
		{
			

			cardInfo = default;
			return false;
		}

		
	}
}