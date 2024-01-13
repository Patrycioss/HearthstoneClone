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
		/// Name of the player.
		/// </summary>
		public string PlayerName { get; private set; }
		
		/// <summary>
		/// Turn object of the player.
		/// </summary>
		public Turn Turn { get; private set; }
		
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
		[SerializeField] private Turn turn;
		

		private List<CardInfo> cardsInHand;

		/// <summary>
		/// Construct a new player with a set of cards in the deck.
		/// </summary>
		/// <param name="playerName">Name of the player.</param>
		/// <param name="cards">The cards the player has.</param>
		public void Instantiate(string playerName, IEnumerable<CardInfo> cards)
		{
			PlayerName = playerName;
			PlayerHand = playerHand;
			Turn = turn;
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
					Debug.Log($"[{PlayerName}]: Drawing card: {card.CardName}");

					PlayerHand.AddCard(card!);
				}
				else
				{
					Debug.Log($"[{PlayerName}]: No more cards to draw!");
				}
			}
		}
	}
}