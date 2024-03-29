﻿using CardComposition;
using Game;
using PhysicalCards;
using UnityEngine;

namespace StateStuff
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is being moved by the player.
	/// </summary>
	public class MovingState : State
	{
		private PhysicalCard card;
		private Transform movingContainer;
		private Player player;
		
		public override void Start()
		{
			StateMachine.GetReference("Card", out card);
			StateMachine.GetReference("MovingContainer", out movingContainer);
			StateMachine.GetReference("Player", out player);
			
			card.transform.parent.SetParent(movingContainer);
		}

		public override void Update()
		{
			card.transform.position = Input.mousePosition;
			
			if (!Input.GetMouseButton(0))
			{
				if (CanBePlayed())
				{
					card.Play();
					StateMachine.SetState(new BoardState());
				}
				else
				{
					StateMachine.SetState(new HeldState());
				}
			}
		}
		
		public override void Stop()
		{
		}

		private bool CanBePlayed()
		{
			if (PlayManager.Instance.Board.DoesPlayerHaveCapacity(card.Owner) || card.CardInfo.Type != CardType.Minion)
			{
				if (player.Mana.TryRemove(card.CardController.Cost))
				{
					return true;
				}

				card.Visuals.ShowError(PhysicalCardVisuals.Error.NotEnoughMana);
				// Debug.LogWarning($"Can't play card because the player doesn't have enough mana!");
			}
			else
			{
				card.Visuals.ShowError(PhysicalCardVisuals.Error.BoardIsFull);
				// Debug.LogWarning($"Can't play minion because the board is full!");
			}
			return false;
		}
	}
}