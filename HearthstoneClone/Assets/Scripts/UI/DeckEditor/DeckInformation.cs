using Deck;
using IOSystem;
using TMPro;
using UI.Generic;
using UnityEngine;

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
		[SerializeField] private ButtonContainer saveButton;
		[SerializeField] private Message message;
		
		private DeckInfo deckInfo;

		/// <summary>
		/// Initializes the <see cref="DeckInformation"/>.
		/// </summary>
		/// <param name="initDeckInfo"><see cref="DeckInfo"/> to display.</param>
		public void Initialize(DeckInfo initDeckInfo)
		{
			deckInfo = initDeckInfo;
			ShouldSave = false;

			SetUpText();
		}

		public void EnableShouldSave()
		{
			ShouldSave = true;
		}
		
		private void OnEnable()
		{
			nameText.onValueChanged.AddListener(OnNameValueChanged);
			descriptionText.onValueChanged.AddListener(OnDescriptionValueChanged);
			saveButton.AddListener(OnSaveButtonClicked);
		}

		private void OnDisable()
		{
			nameText.onValueChanged.RemoveAllListeners();
			descriptionText.onValueChanged.RemoveAllListeners();
			saveButton.RemoveListeners();
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
			if (string.IsNullOrEmpty(deckInfo.Name))
			{
				message.Activate(new MessageConfiguration
				{
					MessageText = "Make sure to give your deck a name before saving!",
					ButtonText = "Continue"
				});
				return;
			}
			
			if (ShouldSave)
			{
				await DiskManager.SaveToDisk(deckInfo);
				ShouldSave = false;
			}
		}

		private void SetUpText()
		{
			nameText.SetTextWithoutNotify(deckInfo.Name);
			descriptionText.SetTextWithoutNotify(deckInfo.Description);
		}
	}
}