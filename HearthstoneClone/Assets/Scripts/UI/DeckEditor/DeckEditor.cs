using Deck;
using UnityEngine;

namespace UI.DeckEditor
{
	/// <summary>
	/// Manages the UI of the deck editor.
	/// </summary>
	public class DeckEditor : MonoBehaviour
	{
		[SerializeField] private DeckInformation deckInformation;

		private DeckInfo activeDeck;
		
		private void Awake()
		{
			activeDeck = GameManager.Instance.DeckManager.ActiveDeck;

			if (activeDeck != null)
			{
				Debug.Log($"Active deck has name {activeDeck.Name} and description {activeDeck.Description}");
			}
			else
			{
				Debug.LogError("No active deck found!");
			}
		}

		private void Start()
		{
			deckInformation.Initialize(activeDeck);
		}
	}
}