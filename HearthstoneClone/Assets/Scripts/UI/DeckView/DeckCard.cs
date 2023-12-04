using System;
using Deck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DeckView
{
	/// <summary>
	/// Used to show the properties of a deck as a card in the <see cref="DeckListViewer"/>.
	/// </summary>
	public class DeckCard : MonoBehaviour
	{
		[SerializeField] private GameObject selectedIndicator;
		
		[SerializeField] private TextMeshProUGUI title;
		[SerializeField] private TextMeshProUGUI description;
		[SerializeField] private Image image;

		[SerializeField] private Button button;

		private DeckInfo info;
		
		/// <summary>
		/// Instantiate the deck card with <see cref="DeckInfo"/>.
		/// </summary>
		/// <param name="receivedInfo"><see cref="DeckInfo"/> necessary to show the correct properties of the deck.</param>
		public void Instantiate(DeckInfo receivedInfo, Action<DeckInfo, DeckCard> onClickCallback)
		{
			info = receivedInfo;
			
			title.text = info.Name;
			description.text = info.Description;
			// image.sprite = info.Image;

			button.onClick.AddListener(() => onClickCallback?.Invoke(info, this));
		}

		/// <summary>
		/// Sets whether the indication whether the <see cref="DeckCard"/> is selected is active.
		/// </summary>
		/// <param name="active">Is the indication active.</param>
		public void SetSelectedIndicatorActive(bool active)
		{
			selectedIndicator.SetActive(active);
		}

		private void Awake()
		{
			selectedIndicator.SetActive(false);
		}
	}
}