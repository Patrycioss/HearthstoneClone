using System.Collections.Generic;
using Deck;
using Deck.DeckManagement;
using UI.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UI.DeckView
{
	/// <summary>
	/// Controls the UI to show the user a list of their decks.
	/// </summary>
	public class DeckListViewer : MonoBehaviour
	{
		[SerializeField] private AssetLabelReference deckCardLabel;
		[SerializeField] private GridLayoutGroup contentTransform;
		[SerializeField] private Scrollbar scrollbar;
		[SerializeField] private SelectedOptions selectedOptions;
		[SerializeField] private GameObject selectedOptionsElement;
		[SerializeField] private ConfirmationScreen confirmationScreen;

		private DiskManager diskManager;
		
		private List<DeckInfo> decks = new();
		private DeckInfo selectedDeck = null;
		private DeckCard selectedDeckCard = null;
		
		private void Start()
		{
			diskManager = GameManager.Instance.DiskManager;
			selectedOptionsElement.SetActive(false);
			LoadDecks();
		}

		private void OnEnable()
		{
			selectedOptions.OnPlayButtonClicked += OnPlayDeckButtonClicked;
			selectedOptions.OnEditButtonClicked += OnEditDeckButtonClicked;
			selectedOptions.OnDeleteButtonClicked += OnDeleteDeckButtonClicked;
		}

		private async void LoadDecks()
		{
			decks = await diskManager.LoadFromDisk<DeckInfo>(SaveDirectory.Decks);
			
			for (int i = 0; i < decks.Count; i++)
			{
				DeckInfo deckInfo = decks[i];

				var deckCardHandle = Addressables.InstantiateAsync(deckCardLabel, contentTransform.transform);
				await deckCardHandle.Task;

				if (deckCardHandle.Status == AsyncOperationStatus.Succeeded)
				{
					GameObject cardObject = deckCardHandle.Result;
					cardObject.name = cardObject.name += $" - {i}";

					if (cardObject.TryGetComponent(out DeckCard deckCard))
					{
						deckCard.Instantiate(deckInfo, OnDeckSelected);
					}
					else Debug.LogError($"Spawned object in {nameof(DeckListViewer)} doesn't have a {nameof(DeckCard)}!");
				}
			}

			scrollbar.value = 1;
		}

		private void OnDisable()
		{
			selectedOptions.OnPlayButtonClicked -= OnPlayDeckButtonClicked;
			selectedOptions.OnEditButtonClicked -= OnEditDeckButtonClicked;
			selectedOptions.OnDeleteButtonClicked -= OnDeleteDeckButtonClicked;
		}
		
		private void OnDeckSelected(DeckInfo deck, DeckCard deckCard)
		{
			if (deck == selectedDeck)
			{
				deckCard.SetSelectedIndicatorActive(false);
				selectedOptionsElement.SetActive(false);
				selectedDeck = null;
				selectedDeckCard = null;
			}
			else 
			{
				if (selectedDeckCard != null)
				{
					selectedDeckCard.SetSelectedIndicatorActive(false);
				}

				selectedOptionsElement.SetActive(true);
				deckCard.SetSelectedIndicatorActive(true);
				selectedDeck = deck;
				selectedDeckCard = deckCard;
			}
		}

		private void OnPlayDeckButtonClicked()
		{
			Debug.Log($"Pressed play button for {selectedDeckCard.name}.");
			
			confirmationScreen.Activate(new ConfirmationScreenConfiguration
			{
				MessageText = $"Are you sure you want to start a game with this deck?",
				OnConfirm = () =>
				{
					GameManager.Instance.SceneSwapper.SetScene(SceneSwapper.Scene.Game);
				}
			});
		}

		private void OnEditDeckButtonClicked()
		{
			Debug.Log($"Pressed edit button for {selectedDeckCard.name}. Setting active deck to {selectedDeck}. Loading deck editor scene...");
			
			GameManager.Instance.AddTransferable("ActiveDeck", selectedDeck);
			GameManager.Instance.SceneSwapper.SetScene(SceneSwapper.Scene.DeckEditor);
		}

		private void OnDeleteDeckButtonClicked()
		{
			Debug.Log($"Pressed delete button for {selectedDeckCard.name}.");
			
			confirmationScreen.Activate(new ConfirmationScreenConfiguration
			{
				MessageText = $"Are you sure you wish to delete this deck?",
				OnConfirm = () =>
				{
					diskManager.RemoveFromDisk(selectedDeck);
					Destroy(selectedDeckCard.gameObject);
				}
			});
		}
	}
}