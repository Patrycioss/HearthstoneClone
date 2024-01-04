using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardManagement.CardComposition;
using CardManagement.Physical;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using Utils;

namespace Game
{
	/// <summary>
	/// Represents the in-game player hand.
	/// </summary>
	public class PlayerHand : MonoBehaviour
	{
		[SerializeField] private AssetLabelReference cardPrefabLabel;
		
		private List<CardInfo> cards = new List<CardInfo>();
		private int maxCardAmount;

		private void Start()
		{
			maxCardAmount = GameManager.Instance.MaxCardsInHand;
		}

		public async void AddCard([NotNull] CardInfo cardInfo)
		{
			if (cards.Count < maxCardAmount)
			{
				cards.Add(cardInfo);
				await SpawnCard(cardInfo);
			}
		}

		private async Task SpawnCard(CardInfo card)
		{
			GameObject cardObject = await ResourceUtils.InstantiateFromLabel(cardPrefabLabel,
				new InstantiationParameters(transform, false));

			if (cardObject.TryGetComponent(out PhysicalCard physicalCard))
			{
				physicalCard.Instantiate(card);
				physicalCard.Flip();
			}
		}
	}
}