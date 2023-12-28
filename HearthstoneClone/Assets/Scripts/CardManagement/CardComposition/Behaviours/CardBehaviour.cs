using UnityEngine;

namespace CardManagement.CardComposition.Behaviours
{
	public abstract class CardBehaviour : ScriptableObject
	{
		/// <summary>
		/// Called when it's the player's turn and the player clicks on the card on the board.
		/// </summary>
		public virtual void OnInteract()
		{
			
		}

		/// <summary>
		/// Should be called in the unity update loop.
		/// </summary>
		public virtual void Update()
		{
			
		}

		/// <summary>
		/// Called whenever the card is used in the hand.
		/// </summary>
		public virtual void OnUse()
		{
			
		}
	}
}