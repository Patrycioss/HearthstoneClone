using Deck;
using TMPro;
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
		
		private DeckInfo deckInfo;

		/// <summary>
		/// Initializes the <see cref="DeckInformation"/>.
		/// </summary>
		/// <param name="deckInfo"><see cref="DeckInfo"/> to display.</param>
		public void Initialize(DeckInfo deckInfo)
		{
			this.deckInfo = deckInfo;
			ShouldSave = false;
			
			SetUpText();
			SetUpCardsView();
		}
		
		private void OnEnable()
		{
			nameText.onValueChanged.AddListener(OnNameValueChanged);
			descriptionText.onValueChanged.AddListener(OnDescriptionValueChanged);
		}

		private void OnDisable()
		{
			nameText.onValueChanged.RemoveAllListeners();
			descriptionText.onValueChanged.RemoveAllListeners();
		}

		private void OnNameValueChanged(string newValue)
		{
			deckInfo.Name = newValue;
			ShouldSave = true;
		}
		
		private void OnDescriptionValueChanged(string newValue)
		{
			deckInfo.Name = newValue;
			ShouldSave = true;
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