using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Deck
{
	/// <summary>
	/// Thumbnail to show the deck in the user's collection.
	/// </summary>
	public class DeckThumbnail : MonoBehaviour
	{
		[SerializeField] private Image background;
		[SerializeField] private TextMeshProUGUI title;
		[SerializeField] private Image image;
		[SerializeField] private TextMeshProUGUI description;

		/// <summary>
		/// Initialize the thumbnail with a <see cref="DeckInfo"/>.
		/// </summary>
		public void Initialize(DeckInfo deckInfo)
		{
			background.color = Random.ColorHSV();
			
			if (string.IsNullOrEmpty(deckInfo.Description))
			{
				description.gameObject.SetActive(false);
			}
			else description.text = deckInfo.Description;

			if (deckInfo.Image == null)
			{
				image.gameObject.SetActive(false);
			}
			else image.sprite = deckInfo.Image;
			
			if (string.IsNullOrEmpty(deckInfo.Name))
			{
				title.gameObject.SetActive(false);
			}
			else title.text = deckInfo.Name;
		}
	}
}