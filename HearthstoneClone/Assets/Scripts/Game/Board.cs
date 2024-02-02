using System;
using System.Collections.Generic;
using CardManagement.Physical;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	/// <summary>
	/// Contains the cards on the board.
	/// </summary>
	public class Board : MonoBehaviour
	{
		/// <summary>
		/// Wrapper object for a card container and its corresponding player.
		/// </summary>
		[Serializable]
		public class CardContainer
		{
			/// <summary>
			/// Owner of the cards.
			/// </summary>
			public Player Owner;
			
			/// <summary>
			/// Transform that is the parent to the cards.
			/// </summary>
			public Transform Container;

			/// <summary>
			/// Cards on the board.
			/// </summary>
			public List<PhysicalCard> Cards { get; set; } = new List<PhysicalCard>();
		}
		
		/// <summary>
		/// The last card the player is hovering over.
		/// </summary>
		[CanBeNull]
		public PhysicalCard CardOfInterest { get; set; }
		
		[SerializeField] private List<CardContainer> containers;
		
		private Dictionary<Player, CardContainer> containerLookup = new Dictionary<Player,CardContainer>();

		/// <summary>
		/// Add a <see cref="PhysicalCard"/> to the board and the internal list.
		/// </summary>
		/// <param name="card">The <see cref="PhysicalCard"/> to add.</param>
		public void AddCard(PhysicalCard card)
		{
			if (containerLookup.TryGetValue(card.Owner, out CardContainer container))
			{
				if (!container.Cards.Contains(card))
				{
					container.Cards.Add(card);
				}
				card.transform.SetParent(container.Container);
			
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) container.Container);
			}
		}
		
		/// <summary>
		/// Tries to remove a <see cref="PhysicalCard"/> from the internal list of the board.
		/// </summary>
		/// <param name="card">The <see cref="PhysicalCard"/></param>
		/// <returns>Whether it could be removed.</returns>
		public bool TryRemoveCard(PhysicalCard card)
		{
			if (containerLookup.TryGetValue(card.Owner, out CardContainer container))
			{
				return container.Cards.Remove(card);
			}

			return false;
		}

		/// <summary>
		/// Returns whether the board has capacity for another card on the player's side.
		/// </summary>
		public bool DoesPlayerHaveCapacity(Player player)
		{
			if (containerLookup.TryGetValue(player, out CardContainer container))
			{
				return container.Cards.Count < GameManager.Instance.MaxBoardSize;
			}

			return false;
		}
		
		/// <summary>
		/// Returns all the cards on the board.
		/// </summary>
		public IEnumerable<PhysicalCard> GetAllCards()
		{
			List<PhysicalCard> cards = new List<PhysicalCard>();

			foreach (CardContainer container in containers)
			{
				cards.AddRange(container.Cards);
			}

			return cards;
		}
		
		private void Awake()
		{
			foreach (CardContainer container in containers)
			{
				containerLookup.TryAdd(container.Owner, container);
			}
		}
	}
}