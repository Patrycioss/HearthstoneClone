using Deck.DeckManagement;
using UnityEngine;
using UnityEngine.UI;

namespace DebugTools
{
	public class DebugButtons : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		
		[SerializeField] private bool useDebugFolder = true;

		[Header("Saving")]
		[SerializeField] private string specificSaveName;
		[SerializeField] private Button saveSpecificButton;
		[SerializeField] private Button saveAllButton;

		[Header("Loading")]
		[SerializeField] private string specificLoadName;
		[SerializeField] private Button loadSpecificButton;
		[SerializeField] private Button loadAllButton;

		private DeckManager deckManager;

		private void Awake()
		{
			deckManager = gameManager.DeckManager;
			
			
			
			saveSpecificButton.onClick.AddListener(OnSaveSpecificButtonClick);
			saveAllButton.onClick.AddListener(OnSaveAllButtonClick);
			loadSpecificButton.onClick.AddListener(OnLoadSpecificButtonClick);
			loadAllButton.onClick.AddListener(OnLoadAllButtonClick);
		}

		private async void OnSaveSpecificButtonClick()
		{
			Debug.Log(await deckManager.SaveDeck(specificSaveName, useDebugFolder));
		}

		private async void OnSaveAllButtonClick()
		{
			Debug.Log(await deckManager.SaveAllDecks(useDebugFolder));
		}

		private async void OnLoadSpecificButtonClick()
		{
			Debug.Log(await deckManager.LoadDeck(specificLoadName, useDebugFolder));
		}

		private async void OnLoadAllButtonClick()
		{
			Debug.Log(await deckManager.LoadAllDecks(useDebugFolder));
		}
	}
}