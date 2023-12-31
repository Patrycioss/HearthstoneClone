﻿using System;
using System.Collections.Generic;
using CardManagement.CardComposition.Behaviours;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CardManagement.CardComposition
{
	[CreateAssetMenu(menuName = "Cards/Card", order = 0)]
	[Serializable]
	public class CardInfo : ScriptableObject
	{
		/// <summary>
		/// Name of the card.
		/// </summary>
		[CanBeNull] 
		public string CardName;

		/// <summary>
		/// Description of the card.
		/// </summary>
		[CanBeNull] 
		public string Description;
		
		/// <summary>
		/// Cost of the card.
		/// </summary>
		public int Cost = 0;
		
		/// <summary>
		/// Addressable asset used for the image.
		/// </summary>
		[CanBeNull]
		public AssetReference ImageReference;
		
		/// <summary>
		/// Behaviour associated with the card.
		/// </summary>
		[NotNull]
		public List<CardBehaviour> Behaviours = new List<CardBehaviour>();
		
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