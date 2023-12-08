using Deck;
using UnityEngine;

namespace UI.DeckEditor
{
	/// <summary>
	/// Manages the deck editor.
	/// </summary>
	public class DeckEditor : MonoBehaviour
	{
		[SerializeField] private DeckInformation deckInformation;

		private GameManager gameManager;

		private void Start()
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
		}
	}
}