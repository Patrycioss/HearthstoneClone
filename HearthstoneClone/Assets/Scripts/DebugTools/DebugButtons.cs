using System.Collections.Generic;
using Deck;
using Deck.DeckManagement;
using UnityEngine;
using UnityEngine.UI;

namespace DebugTools
{
	public class DebugButtons : MonoBehaviour
	{
		[Header("Saving")]
		[SerializeField] private DeckInfo deckToSave;
		[SerializeField] private Button saveSpecificButton;
		[SerializeField] private List<DeckInfo> decksToSave;
		[SerializeField] private Button saveAllButton;

		[Header("Loading")]
		[SerializeField] private string deckToLoad;
		[SerializeField] private SaveDirectory directoryToLoadFrom;
		[SerializeField] private Button loadSpecificButton;
		[SerializeField] private Button loadAllButton;

		private DiskManager diskManager;

		private void Awake()
		{
			saveSpecificButton.onClick.AddListener(OnSaveSpecificButtonClick);
			saveAllButton.onClick.AddListener(OnSaveAllButtonClick);
			loadSpecificButton.onClick.AddListener(OnLoadSpecificButtonClick);
			loadAllButton.onClick.AddListener(OnLoadAllButtonClick);
		}

		private void Start()
		{
			diskManager = GameManager.Instance.DiskManager;
		}

		private async void OnSaveSpecificButtonClick()
		{
			await diskManager.SaveToDisk(deckToSave);
		}

		private async void OnSaveAllButtonClick()
		{
			await diskManager.SaveToDisk(decksToSave);
		}

		private async void OnLoadSpecificButtonClick()
		{
			if (!string.IsNullOrEmpty(deckToLoad))
			{
				DeckInfo deck = await diskManager.LoadFromDisk<DeckInfo>(deckToLoad)!;
				if (deck != null)
				{
					Debug.Log($"Loaded {deck}");
				}
			}
		}

		private async void OnLoadAllButtonClick()
		{
			List<DeckInfo> loadedDecks = await diskManager.LoadFromDisk<DeckInfo>(directoryToLoadFrom);
			foreach (DeckInfo deck in loadedDecks)
			{
				if (deck != null)
				{
					Debug.Log($"Loaded {deck}");
				}
			}
		}
	}
}