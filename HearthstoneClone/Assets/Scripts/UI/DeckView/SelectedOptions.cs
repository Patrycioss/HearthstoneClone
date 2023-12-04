using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DeckView
{
	/// <summary>
	/// Handles the navigation in the selected options screen in the <see cref="DeckListViewer"/>.
	/// </summary>
	public class SelectedOptions : MonoBehaviour
	{
		/// <summary>
		/// Event that's called when the play deck button is clicked.
		/// </summary>
		public event Action OnPlayButtonClicked;

		/// <summary>
		/// Event that's called when the edit deck button is clicked.
		/// </summary>
		public event Action OnEditButtonClicked;

		/// <summary>
		/// Event that's called when the delete deck button is clicked.
		/// </summary>
		public event Action OnDeleteButtonClicked;

		[SerializeField] private Button playButton;
		[SerializeField] private Button editButton;
		[SerializeField] private Button deleteButton;

		private void OnEnable()
		{
			playButton.onClick.AddListener(() => OnPlayButtonClicked?.Invoke());
			editButton.onClick.AddListener(() => OnEditButtonClicked?.Invoke());
			deleteButton.onClick.AddListener(() => OnDeleteButtonClicked?.Invoke());
		}

		private void OnDisable()
		{
			playButton.onClick.RemoveAllListeners();
			editButton.onClick.RemoveAllListeners();
			deleteButton.onClick.RemoveAllListeners();
		}
	}
}