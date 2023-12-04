using System.Collections.Generic;
using System.Linq;
using Deck;
using ErrorHandling;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DeckView
{
	public class DeckListViewer : MonoBehaviour
	{
		[SerializeField] private GameObject deckCardPrefab;
		
		[SerializeField] private GridLayoutGroup contentTransform;
		[SerializeField] private Scrollbar scrollbar;
	
		[SerializeField] private SelectedOptions selectedOptions;
		[SerializeField] private GameObject selectedOptionsElement;

		private List<DeckInfo> decks = new();
		
		private DeckInfo selectedDeck = null;
		private DeckCard selectedDeckCard = null;

		private void Awake()
		{
			LoadDecks();
			selectedOptionsElement.SetActive(false);

			selectedOptions.OnPlayButtonClicked += OnPlayDeckButtonClicked;
			selectedOptions.OnEditButtonClicked += OnEditDeckButtonClicked;
			selectedOptions.OnDeleteButtonClicked += OnDeleteDeckButtonClicked;
		}

		private async void LoadDecks()
		{
			Result a = await GameManager.Instance.DeckManager.LoadAllDecks();
			Debug.Log($"Logging {a.Message} from {nameof(DeckListViewer)} in {nameof(LoadDecks)}");

			decks = GameManager.Instance.DeckManager.GetAllDecks().ToList();

			for (int i = 0; i < decks.Count; i++)
			{
				DeckInfo deckInfo = decks[i];
				GameObject cardObject = Instantiate(deckCardPrefab, contentTransform.transform);
				cardObject.name = cardObject.name += $" - {i}";

				if (cardObject.TryGetComponent(out DeckCard deckCard))
				{
					deckCard.Instantiate(deckInfo, OnDeckSelected);
				}
				else Debug.LogError($"Spawned object in {nameof(DeckListViewer)} doesn't have a {nameof(DeckCard)}!");
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
			// Todo: implement giving deckinfo to confirmation that then starts game with deck.
		}

		private void OnEditDeckButtonClicked()
		{
			Debug.Log($"Pressed edit button for {selectedDeckCard.name}.");
			// Todo: implement edit window to give deckinfo to.
		}

		private void OnDeleteDeckButtonClicked()
		{
			Debug.Log($"Pressed delete button for {selectedDeckCard.name}.");
			// Todo : implement giving deckinfo to confirmation screen that then deletes it.
		}
	}
}