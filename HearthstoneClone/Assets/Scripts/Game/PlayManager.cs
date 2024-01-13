using System;
using System.Collections.Generic;
using CardManagement.CardComposition;
using Deck;
using UnityEngine;
using Utils;

namespace Game
{
	public class PlayManager : MonoBehaviour
	{
		[SerializeField] private DeckInfo testDeck;
		[SerializeField] private Player player1;
		[SerializeField] private Player player2;
		
		private async void Start()
		{
			DeckInfo deck = testDeck;
			if (GameManager.Instance.GetTransferable("ActiveDeck")?.Value is DeckInfo foundDeck)
			{
				deck = foundDeck;
			}
			
			IList<CardInfo> cards = await ResourceUtils.LoadAddressablesFromIdentifiers<CardInfo>(deck.Cards);
			player1.Instantiate("Player 1",cards);
			player2.Instantiate("Player 2",cards);
			
			player1.Turn.OnEndCallback += OnPlayer1TurnComplete;
			player1.Turn.OnStartCallback += OnPlayer1TurnStart;
			player2.Turn.OnEndCallback += OnPlayer2TurnComplete;
			player2.Turn.OnStartCallback += OnPlayer2TurnStart;

			StartGame();
		}

		private void StartGame()
		{
			player1.DrawCard(3);
			player2.DrawCard(3);
			
			player1.Turn.Begin();
		}

		private void OnDestroy()
		{
			player1.Turn.OnEndCallback -= OnPlayer1TurnComplete;
			player1.Turn.OnStartCallback -= OnPlayer1TurnStart;
			player2.Turn.OnEndCallback -= OnPlayer2TurnComplete;
			player2.Turn.OnStartCallback -= OnPlayer2TurnStart;
		}

		private void OnPlayer1TurnComplete()
		{
			player2.Turn.Begin();
			player2.PlayerHand.IsLocked = true;
		}

		private void OnPlayer1TurnStart()
		{
			player1.DrawCard(1);
			player1.PlayerHand.IsLocked = false;
		}
		
		private void OnPlayer2TurnComplete()
		{
			player1.Turn.Begin();
			player1.PlayerHand.IsLocked = true;
		}
		
		private void OnPlayer2TurnStart()
		{
			player2.DrawCard(1);
			player2.PlayerHand.IsLocked = false;
		}
	}
}