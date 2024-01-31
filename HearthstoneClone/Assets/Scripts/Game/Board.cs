using System.Collections.Generic;
using System.Linq;
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
		/// A read-only collection of all the cards on the board.
		/// </summary>
		public IReadOnlyCollection<PhysicalCard> Cards => cards;

		/// <summary>
		/// Container to put temporary UI stuff in.
		/// </summary>
		public GameObject DrawingContainer => drawingContainer;
		
		/// <summary>
		/// Returns whether the mouse is hovering over the board.
		/// </summary>
		public bool IsMouseHovering =>  RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);
		
		/// <summary>
		/// Returns whether the board has capacity for another card.
		/// </summary>
		public bool HasCapacity => cards.Count < GameManager.Instance.MaxBoardSize;
		
		[SerializeField] private GameObject cardContainer;
		[SerializeField] private GameObject drawingContainer;
		
		private List<PhysicalCard> cards = new List<PhysicalCard>();

		private RectTransform rectTransform;

		/// <summary>
		/// Finds the first card on the board that the player is hovering over.
		/// </summary>
		/// <returns>The first card on the board that the player is hovering over.</returns>
		[CanBeNull]
		public PhysicalCard GetFirstCardPlayerIsHoveringOver()
		{
			return cards.FirstOrDefault(card => card.IsHoveringOver);
		}
		
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
			
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) cardContainer.transform);
		}
		
		/// <summary>
		/// Tries to remove a <see cref="PhysicalCard"/> from the internal list of the board.
		/// </summary>
		/// <param name="card">The <see cref="PhysicalCard"/></param>
		/// <returns>Whether it could be removed.</returns>
		public bool TryRemoveCard(PhysicalCard card)
		{
			return cards.Remove(card);
		}
		
		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}
	}
}