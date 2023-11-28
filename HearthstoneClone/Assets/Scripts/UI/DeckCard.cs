using Deck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	/// <summary>
	/// Used to show the properties of a deck as a card in the <see cref="DeckListViewer"/>.
	/// </summary>
	public class DeckCard : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI title;
		[SerializeField] private TextMeshProUGUI description;
		[SerializeField] private Image image;
		
		/// <summary>
		/// Instantiate the deck card with <see cref="DeckInfo"/>.
		/// </summary>
		/// <param name="receivedInfo"><see cref="DeckInfo"/> necessary to show the correct properties of the deck.</param>
		public void Instantiate(DeckInfo receivedInfo)
		{
			title.text = receivedInfo.Name;
			description.text = receivedInfo.Description;
			// image.sprite = receivedInfo.Image;
		}
	}
}