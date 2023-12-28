using System.Collections.Generic;
using System.Threading.Tasks;
using CardManagement.CardComposition;
using CardManagement.Physical;
using Deck;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UI.DeckEditor
{
	/// <summary>
	/// Manages the deck editor.
	/// </summary>
	public class DeckEditor : MonoBehaviour
	{
		[SerializeField] private AssetLabelReference cardsLabel;
		[SerializeField] private AssetLabelReference cardPrefabLabel;
		
		[SerializeField] private GridLayoutGroup gridLayoutGroup;
		
		[SerializeField] private DeckInformation deckInformation;

		private GameManager gameManager;
		private List<CardInfo> cards = new List<CardInfo>();

		private List<GameObject> physicalCards = new List<GameObject>();

		private Task loadingTask;

		private async void Start()
		{
			
			DeckInfo deck = GameManager.Instance.GetTransferable("ActiveDeck") as DeckInfo;

			if (deck == null)
			{
				Debug.LogError($"No active deck found in GameManager!");
			}
			else
			{
				deckInformation.Initialize(deck);
			}

			await LoadCards();

			foreach (CardInfo card in cards)
			{
				var handle = Addressables.InstantiateAsync(cardPrefabLabel);
				await handle.Task;
				
				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					handle.Result.transform.parent = gridLayoutGroup.transform;
					
					if (handle.Result.TryGetComponent(out PhysicalCard physicalCard))
					{
						physicalCard.Instantiate(card);
					}
					
					physicalCards.Add(handle.Result);
				}
				else
				{
					Debug.LogError($"Failed to load cards for deck editor.");
				}
			}
		}

		private async Task LoadCards()
		{
			var handle = Addressables.LoadAssetsAsync<CardInfo>(cardsLabel, _ => { });
			await handle.Task;

			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				cards = new List<CardInfo>(handle.Result);
			}
			else
			{
				Debug.LogError($"Failed to load cards for deck editor.");
			}
		}
	}
}