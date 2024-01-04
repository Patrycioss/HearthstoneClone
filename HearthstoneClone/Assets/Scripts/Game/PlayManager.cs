using System.Collections.Generic;
using CardManagement.CardComposition;
using Deck;
using UnityEngine;
using Utils;

namespace Game
{
	public class PlayManager : MonoBehaviour
	{
		[SerializeField] private Player player1;
		[SerializeField] private Player player2;
		

		private async void Start()
		{
			if (GameManager.Instance.GetTransferable("ActiveDeck") is DeckInfo deck)
			{
				IList<CardInfo> cards = await ResourceUtils.LoadAddressablesFromIdentifiers<CardInfo>(deck.Cards);
				player1.Instantiate(cards);
				player2.Instantiate(cards);
			}
			
			StartGame();
		}

		private void StartGame()
		{
			
		}
	}
}