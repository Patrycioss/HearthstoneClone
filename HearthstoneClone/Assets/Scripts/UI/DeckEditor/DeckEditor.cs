using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardManagement.CardComposition;
using Deck;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;
using Utils;

namespace UI.DeckEditor
{
	/// <summary>
	/// Manages the deck editor.
	/// </summary>
	public class DeckEditor : MonoBehaviour
	{
		
		[SerializeField] private AssetLabelReference cardsLabel;
		[SerializeField] private AssetLabelReference cardPrefabLabel;
		[SerializeField] private AssetLabelReference cardLayerPrefabLabel;
		
		[SerializeField] private GridLayoutGroup gridLayoutGroup;
		[SerializeField] private VerticalLayoutGroup stackVerticalLayoutGroup;
		
		
		[SerializeField] private DeckInformation deckInformation;

		private GameManager gameManager;
		private List<CardInfo> cards = new List<CardInfo>();

		private List<GameObject> physicalCards = new List<GameObject>();

		private Task loadingTask;
		private DeckInfo activeDeck;

		private Dictionary<CardInfo, string> infoToLocation = new Dictionary<CardInfo, string>();
		private Dictionary<string, CardInfo> locationToInfo = new Dictionary<string, CardInfo>();


		private async void Start()
		{
			if (GameManager.Instance.GetTransferable("ActiveDeck").Value is not DeckInfo deck)
			{
				Debug.LogError($"No active deck found in GameManager!");
			}
			else
			{
				deckInformation.Initialize(deck);
				activeDeck = deck;
			}
			
			List<(CardInfo, IResourceLocation)> loadedCards = await LoadCards();

			foreach ((CardInfo info, IResourceLocation location) loaded in loadedCards)
			{
				Debug.Log($"Added {loaded.location} and {loaded.info}");
				locationToInfo.TryAdd(loaded.location.InternalId, loaded.info);
				infoToLocation.TryAdd(loaded.info, loaded.location.InternalId);
			}

			MakeLayers(activeDeck.Cards);

			await InstantiateCards(loadedCards);
		}
		

		private async Task<List<(CardInfo, IResourceLocation)>> LoadCards()
		{
			var locationsHandle = Addressables.LoadResourceLocationsAsync(cardsLabel);
			await locationsHandle.Task;

			if (locationsHandle.Status == AsyncOperationStatus.Succeeded)
			{
				var locations = locationsHandle.Result;

				var cardInfos = await ResourceUtils.LoadAddressables<CardInfo>(cardsLabel);
				
				
				return cardInfos.Select((cardInfo, index) => (t: cardInfo, locations[index])).ToList();
			}
			

			Debug.LogError($"Failed to load cards for deck editor.");
			return new List<(CardInfo, IResourceLocation)>();
		}

		private void OnCardPressed(CardInfo cardInfo, IResourceLocation resourceLocation)
		{
			if (activeDeck.AddCard(resourceLocation.InternalId))
			{
				MakeLayer(cardInfo);
				deckInformation.EnableShouldSave();	
			}
			else
			{
				//Todo: show deck is full message.
			}
		}

		private void OnLayerPressed(CardInfo cardInfo, GameObject layerObject)
		{
			if (infoToLocation.TryGetValue(cardInfo, out string internalId))
			{
				activeDeck.Cards.Remove(internalId);
			}
			
			Addressables.ReleaseInstance(layerObject);
			deckInformation.EnableShouldSave();
		}

		private void MakeLayers(List<string> cardIdentifiers)
		{
			foreach (string cardIdentifier in cardIdentifiers)
			{
				IResourceLocation location = ResourceUtils.GetLocation(cardIdentifier);
				
				if (locationToInfo.TryGetValue(location.InternalId, out CardInfo cardInfo))
				{
					MakeLayer(cardInfo);
				}
				else Debug.LogWarning($"Can't find location {location} in locationToInfo");
			}	
		}
		
		private async void MakeLayer(CardInfo cardInfo)
		{
			var handle = Addressables.InstantiateAsync(cardLayerPrefabLabel);
			await handle.Task;
				
			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				handle.Result.transform.SetParent(stackVerticalLayoutGroup.transform);
					
				if (handle.Result.TryGetComponent(out CardLayer cardLayer))
				{
					cardLayer.Initialize(cardInfo, OnLayerPressed);
				}
					
				physicalCards.Add(handle.Result);
			}
			else
			{
				Debug.LogError($"Failed to load cards for deck editor.");
			}
		}

		private async Task InstantiateCards(List<(CardInfo, IResourceLocation)> cardLocationPairs)
		{
			foreach ((CardInfo info, IResourceLocation location) card in cardLocationPairs)
			{
				await InstantiateCard(card.info, card.location);
			}
		}

		private async Task InstantiateCard(CardInfo cardInfo, IResourceLocation resourceLocation)
		{
			var handle = Addressables.InstantiateAsync(cardPrefabLabel);
			await handle.Task;
				
			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				handle.Result.transform.SetParent(gridLayoutGroup.transform);
					
				if (handle.Result.TryGetComponent(out UICard uiCard))
				{
					uiCard.Initialize(cardInfo, resourceLocation, OnCardPressed);
				}
					
				physicalCards.Add(handle.Result);
			}
			else
			{
				Debug.LogError($"Failed to load cards for deck editor.");
			}
		}
	}
}