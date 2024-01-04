using System.Collections.Generic;
using System.Linq;
using CardManagement.CardComposition;
using UI;
using UnityEngine;
using Utils;

namespace Game
{
	/// <summary>
	/// Object containing all the information of a character in the game.
	/// </summary>
	public class Player : MonoBehaviour
	{
		/// <summary>
		/// Health of the player.
		/// </summary>
		public ResourceContainer Health { get; private set; }
		
		/// <summary>
		/// Deck that the player uses.
		/// </summary>
		public PlayerDeck PlayerDeck { get; private set; }
		
		/// <summary>
		/// Hand of the player.
		/// </summary>
		public PlayerHand PlayerHand { get; private set; }
		
		/// <summary>
		/// Mana bar of the player.
		/// </summary>
		public ManaBar ManaBar { get; private set; }

		[SerializeField] private PlayerHand playerHand;
		[SerializeField] private ManaBar manaBar;

		private List<CardInfo> cardsInHand;
		
		/// <summary>
		/// Construct a new player with a set of cards in the deck.
		/// </summary>
		/// <param name="cards"></param>
		public void Instantiate(IEnumerable<CardInfo> cards)
		{
			PlayerHand = playerHand;
			ManaBar = manaBar;
			Health = new ResourceContainer(GameManager.Instance.PlayerStartHealth);
			PlayerDeck = new PlayerDeck(CollectionUtils.RandomizeList(cards.ToList()));
		}

		/// <summary>
		/// Draw cards from <see cref="PlayerDeck"/> and put them in <see cref="PlayerHand"/>.
		/// </summary>
		/// <param name="amount">The amount of cards.</param>
		public void DrawCard(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				if (PlayerDeck.GetCard(out CardInfo card))
				{
					PlayerHand.AddCard(card!);
				}
			}
		}
	}
}