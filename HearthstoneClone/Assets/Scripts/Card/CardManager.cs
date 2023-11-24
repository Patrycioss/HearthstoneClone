using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Card
{
	/// <summary>
	/// Used to add cards to the game and allows for the game to find them.
	/// </summary>
	public class CardManager : MonoBehaviour
	{
		[SerializeField] private List<CardInfo> cards;

		/// <summary>
		/// Attempts to return the card with the provided name.
		/// </summary>
		/// <param name="cardName">Name of the card.</param>
		/// <returns></returns>
		[CanBeNull]
		public CardInfo? GetCard(string cardName)
		{
			return cards.Find(info => info.Name == cardName);
		}
	}
}