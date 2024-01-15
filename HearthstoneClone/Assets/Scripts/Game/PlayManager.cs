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
			player1.Initialize("Player 1",cards);
			player2.Initialize("Player 2",cards);
			
			player1.Turn.OnEndCallback += OnPlayer1TurnComplete;
			player1.Turn.OnStartCallback += OnPlayer1TurnStart;
			player2.Turn.OnEndCallback += OnPlayer2TurnComplete;
			player2.Turn.OnStartCallback += OnPlayer2TurnStart;

			StartGame();
		}

		private async void StartGame()
		{
			await player1.DrawCard(3);
			await player2.DrawCard(3);
			
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
			player2.IsLocked = true;
		}

		private async void OnPlayer1TurnStart()
		{
			Debug.Log($"booger");
			await player1.DrawCard(1);
			player1.IsLocked = false;
			player2.IsLocked = true;
		}
		
		private void OnPlayer2TurnComplete()
		{
			player1.Turn.Begin();
			player1.IsLocked = true;
		}
		
		private async void OnPlayer2TurnStart()
		{
			await player2.DrawCard(1);
			player2.IsLocked = false;
			player1.IsLocked = true;
		}
	}
}