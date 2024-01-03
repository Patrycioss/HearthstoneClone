using System;
using System.Threading.Tasks;
using CardManagement.CardComposition;
using Deck;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using Utils;
using Random = UnityEngine.Random;

namespace Game
{
	public class Hand : MonoBehaviour
	{
		public event Action OnDeckEmtpy;
		
		[SerializeField] private GameObject deckPosition;
		[SerializeField] private AssetLabelReference cardPrefabLabel;
		
		
		private DeckInfo deck;
		private CardInfo cards;
		
		public void Initialize(DeckInfo deckInfo)
		{
			deck = deckInfo;
		}

		public async void Draw(int amount = 1)
		{
			for (int i = 0; i < amount; i++)
			{
				if (deck.Cards.Count == 0)
				{
					OnDeckEmtpy?.Invoke();
					return;
				}
				
				string identifier = deck.Cards[Random.Range(0, deck.Cards.Count)];
				await SpawnCard(identifier);
			}
		}

		private async Task SpawnCard(string cardIdentifier)
		{
			CardInfo card = await ResourceUtils.LoadAddressableFromIdentifier<CardInfo>(cardIdentifier);

			if (card != null)
			{
				GameObject spawnedCard = await ResourceUtils.InstantiateFromLabel(cardPrefabLabel,
					new InstantiationParameters(transform, false));
			}
			
		}
	}
}