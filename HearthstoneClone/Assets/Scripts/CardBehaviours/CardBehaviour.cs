﻿using CustomLogging;
using UnityEngine;

namespace CardBehaviours
{
	/// <summary>
	/// Behaviour associated with a card.
	/// </summary>
	public class CardBehaviour : MonoBehaviour
	{
		protected bool Selected;
		
		/// <summary>
		/// Called whenever the card is played from the hand.
		/// </summary>
		public virtual void OnPlay()
		{
			TimedLogger.Log($"Behaviour of type {GetType()} OnPlay called!");
		}

		/// <summary>
		/// Called every frame.
		/// </summary>
		public virtual void Update()
		{

		}

		/// <summary>
		/// Called when it's the player's turn and the player selects the card.
		/// </summary>
		public virtual void OnSelect()
		{
			Selected = true;
			TimedLogger.Log($"Behaviour of type {GetType()} OnSelect called!");
		}
		
		/// <summary>
		/// Called when it's the player's turn and the player deselects the card.
		/// </summary>
		public virtual void OnDeselect()
		{
			Selected = false;
			TimedLogger.Log($"Behaviour of type {GetType()} OnDeselect called!");
		}
	}
}