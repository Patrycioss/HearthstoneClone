using CardManagement.Physical;
using CustomLogging;
using UnityEngine;

namespace CardManagement.CardComposition.Behaviours
{
	/// <summary>
	/// Behaviour associated with a card.
	/// </summary>
	public abstract class CardBehaviour : MonoBehaviour
	{
		protected PhysicalCard Card;
		protected TimedLogger Logger;
		protected GameObject Container;
		
		/// <summary>
		/// Called whenever the card is played from the hand.
		/// </summary>
		public virtual void OnPlay()
		{
			Logger.Log($"Behaviour of type {GetType()} OnPlay called!");
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
			Logger.Log($"Behaviour of type {GetType()} OnSelect called!");

		}
		
		/// <summary>
		/// Called when it's the player's turn and the player deselects the card.
		/// </summary>
		public virtual void OnDeselect()
		{
			Logger.Log($"Behaviour of type {GetType()} OnDeselect called!");
		}
	}
}