using System.Collections.Generic;
using Deck;
using IOSystem;
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

		private void Awake()
		{
			saveSpecificButton.onClick.AddListener(OnSaveSpecificButtonClick);
			saveAllButton.onClick.AddListener(OnSaveAllButtonClick);
			loadSpecificButton.onClick.AddListener(OnLoadSpecificButtonClick);
			loadAllButton.onClick.AddListener(OnLoadAllButtonClick);
		}

		private async void OnSaveSpecificButtonClick()
		{
			await DiskManager.SaveToDisk(deckToSave);
		}

		private async void OnSaveAllButtonClick()
		{
			await DiskManager.SaveToDisk(decksToSave);
		}

		private async void OnLoadSpecificButtonClick()
		{
			if (!string.IsNullOrEmpty(deckToLoad))
			{
				DeckInfo deck = await DiskManager.LoadFromDisk<DeckInfo>(deckToLoad)!;
				if (deck != null)
				{
					Debug.Log($"Loaded {deck}");
				}
			}
		}

		private async void OnLoadAllButtonClick()
		{
			List<DeckInfo> loadedDecks = await DiskManager.LoadFromDisk<DeckInfo>(directoryToLoadFrom);
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