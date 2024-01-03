using System.Collections.Generic;
using CardManagement.CardComposition;
using Deck;
using UnityEngine;
using Utils;

namespace Game
{
	public class PlayManager : MonoBehaviour
	{
		private Player player1;
		private Player player2;

		private async void Start()
		{
			if (GameManager.Instance.GetTransferable("ActiveDeck") is DeckInfo deck)
			{
				IList<CardInfo> cards = await ResourceUtils.LoadAddressablesFromIdentifiers<CardInfo>(deck.Cards);

				player1 = new Player(cards);
				player2 = new Player(cards);
			}
		}
	}
}