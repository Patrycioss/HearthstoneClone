using System;
using UnityEngine;

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

		[SerializeField] private ButtonContainer playButton;
		[SerializeField] private ButtonContainer editButton;
		[SerializeField] private ButtonContainer deleteButton;

		private void OnEnable()
		{
			playButton.AddListener(() => OnPlayButtonClicked?.Invoke());
			editButton.AddListener(() => OnEditButtonClicked?.Invoke());
			deleteButton.AddListener(() => OnDeleteButtonClicked?.Invoke());
		}

		private void OnDisable()
		{
			playButton.RemoveListeners();
			editButton.RemoveListeners();
			deleteButton.RemoveListeners();
		}
	}
}