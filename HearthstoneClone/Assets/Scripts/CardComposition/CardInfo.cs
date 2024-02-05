using System;
using System.Collections.Generic;
using CustomEditor;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CardComposition
{
	/// <summary>
	/// Contains the information associated with a card.
	/// <remarks>Correct properties in the editor are shown using <see cref="CardInfoEditor"/>.</remarks>
	/// </summary>
	[CreateAssetMenu(menuName = "Cards/Card", order = 0)]
	[Serializable]
	public class CardInfo : ScriptableObject
	{
		/// <summary>
		/// Name of the card.
		/// </summary>
		[CanBeNull] 
		public string CardName;

		// /// <summary>
		// /// Description of the card.
		// /// </summary>
		// [CanBeNull] 
		// public string Description;
		
		/// <summary>
		/// Cost of the card.
		/// </summary>
		public int Cost = 0;
		
		/// <summary>
		/// Addressable asset used for the image.
		/// </summary>
		[CanBeNull]
		public AssetReference ImageReference;
		
		// /// <summary>
		// /// Behaviour references associated with the card.
		// /// </summary>
		// public List<AssetReference> CardBehaviourReferences = new List<AssetReference>();

		public AssetReference CardBehaviour;
		
		/// <summary>
		/// Type of the card.
		/// </summary>
		public CardType Type = CardType.Minion;
		
		/// <summary>
		/// Minion health and weapon durability.
		/// </summary>
		public int Health = 1;
		
		/// <summary>
		/// Used for weapon dmg, minion dmg and spell dmg.
		/// </summary>
		public int Attack = 1;
		
		/// <summary>
		/// Special attributes that a minion can have.
		/// </summary>
		[NotNull] 
		public List<CardAttribute> MinionAttributes = new List<CardAttribute>();
	}
}