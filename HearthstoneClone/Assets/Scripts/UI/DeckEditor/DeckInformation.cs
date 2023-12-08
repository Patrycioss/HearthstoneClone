using Deck;
using Deck.DeckManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DeckEditor
{
	/// <summary>
	/// Displays information about the deck like the name, description and what cards it has.
	/// </summary>
	public class DeckInformation : MonoBehaviour
	{
		/// <summary>
		/// If the user has made any changes they are suggested to save.
		/// </summary>
		public bool ShouldSave {get; private set;}
		
		[SerializeField] private TMP_InputField nameText;
		[SerializeField] private TMP_InputField descriptionText;
		[SerializeField] private Button saveButton;
		
		private DiskManager diskManager;

		private DeckInfo deckInfo;

		/// <summary>
		/// Initializes the <see cref="DeckInformation"/>.
		/// </summary>
		/// <param name="initDeckInfo"><see cref="DeckInfo"/> to display.</param>
		public void Initialize(DeckInfo initDeckInfo)
		{
			deckInfo = initDeckInfo;
			diskManager = GameManager.Instance.DiskManager;
			ShouldSave = false;
			
			SetUpText();
			SetUpCardsView();
		}
		
		private void OnEnable()
		{
			nameText.onValueChanged.AddListener(OnNameValueChanged);
			descriptionText.onValueChanged.AddListener(OnDescriptionValueChanged);
			saveButton.onClick.AddListener(OnSaveButtonClicked);
		}

		private void OnDisable()
		{
			nameText.onValueChanged.RemoveAllListeners();
			descriptionText.onValueChanged.RemoveAllListeners();
			saveButton.onClick.RemoveAllListeners();
		}

		private void OnNameValueChanged(string newValue)
		{
			deckInfo.Name = newValue;
			ShouldSave = true;
		}
		
		private void OnDescriptionValueChanged(string newValue)
		{
			deckInfo.Description = newValue;
			ShouldSave = true;
		}

		private async void OnSaveButtonClicked()
		{
			if (ShouldSave)
			{
				await diskManager.SaveToDisk(deckInfo);
				ShouldSave = false;
			}
		}

		private void SetUpText()
		{
			nameText.SetTextWithoutNotify(deckInfo.Name);
			descriptionText.SetTextWithoutNotify(deckInfo.Description);
		}

		private void SetUpCardsView()
		{
			
		}
	}
}