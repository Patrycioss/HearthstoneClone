using System;
using System.Collections.Generic;
using CardManagement.CardComposition;
using UnityEngine;

namespace CardManagement
{
	[CreateAssetMenu(fileName = "CardCollection", menuName = "Cards/Collection", order = 0)]
	[Serializable]
	public class CardCollection : ScriptableObject
	{
		[SerializeField] private List<CardInfo> cards = new List<CardInfo>();
	}
}