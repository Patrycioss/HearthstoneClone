using System.Collections.Generic;
using CardManagement.Physical;
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
		/// A read-only collection of all the cards on the board.
		/// </summary>
		public IReadOnlyCollection<PhysicalCard> Cards => cards;
		
		/// <summary>
		/// Returns whether the mouse is hovering over the board.
		/// </summary>
		public bool IsMouseHovering =>  RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);
		
		/// <summary>
		/// Returns whether the board has capacity for another card.
		/// </summary>
		public bool HasCapacity => cards.Count < GameManager.Instance.MaxBoardSize;
		
		[SerializeField] private GameObject cardContainer;
		
		private List<PhysicalCard> cards = new List<PhysicalCard>();

		private RectTransform rectTransform;
		
		/// <summary>
		/// Add a <see cref="PhysicalCard"/> to the board and the internal list.
		/// </summary>
		/// <param name="card">The <see cref="PhysicalCard"/> to add.</param>
		public void TryAddCard(PhysicalCard card)
		{
			if (!cards.Contains(card))
			{
				cards.Add(card);
			}
			card.transform.SetParent(cardContainer.transform);
			card.IsOnBoard = true;
			
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) cardContainer.transform);
		}
		
		/// <summary>
		/// Tries to remove a <see cref="PhysicalCard"/> from the internal list of the board.
		/// </summary>
		/// <param name="card">The <see cref="PhysicalCard"/></param>
		/// <returns>Whether it could be removed.</returns>
		public bool TryRemoveCard(PhysicalCard card)
		{
			if (cards.Remove(card))
			{
				card.IsOnBoard = false;
				return true;
			}

			return false;
		}
		
		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}
	}
}