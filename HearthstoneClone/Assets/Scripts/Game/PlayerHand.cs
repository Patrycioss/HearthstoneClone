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
		/// <summary>
		/// Can cards be moved?
		/// </summary>
		public bool IsLocked
		{
			set
			{
				isLocked = value;

				foreach (PhysicalCard card in physicalCards)
				{
					card.IsLocked = isLocked;
				}
			}
		}
		
		[SerializeField] private AssetLabelReference cardPrefabLabel;
		
		private int maxCardAmount;

		private bool isLocked;

		private List<PhysicalCard> physicalCards = new List<PhysicalCard>();
		private Player.TryPlayCallback tryPlayCallback;

		/// <summary>
		/// Initialize the player hand.
		/// </summary>
		/// <param name="playCallback">The callback to call when a card is tried to play.</param>
		public void Initialize(Player.TryPlayCallback playCallback)
		{
			tryPlayCallback = playCallback;
		}

		/// <summary>
		/// Add a card to the player hand.
		/// </summary>
		/// <param name="cardInfo">Info associated with the card.</param>
		public async void AddCard([NotNull] CardInfo cardInfo)
		{
			if (physicalCards.Count < maxCardAmount)
			{
				await SpawnCard(cardInfo);
			}
		}
		
		private void Start()
		{
			maxCardAmount = GameManager.Instance.MaxCardsInHand;
		}

		private async Task SpawnCard(CardInfo card)
		{
			GameObject cardObject = await ResourceUtils.InstantiateFromLabel(cardPrefabLabel,
				new InstantiationParameters(transform, false));

			if (cardObject.TryGetComponent(out PhysicalCard physicalCard))
			{
				physicalCard.Initialize(card, tryPlayCallback);
				physicalCard.Flip();
				
				physicalCards.Add(physicalCard);
			}
		}
	}
}