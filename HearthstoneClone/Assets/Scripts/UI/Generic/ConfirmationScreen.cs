using System;
using TMPro;
using UnityEngine;

namespace UI.Generic
{
	/// <summary>
	/// Script that steers the confirmation screen for exiting the game for example.
	/// </summary>
	public class ConfirmationScreen : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI messageText;
		[SerializeField] private ButtonContainer confirmButton;
		[SerializeField] private ButtonContainer cancelButton;
		[SerializeField] private ConfirmationScreenConfiguration defaultConfiguration;

		private ConfirmationScreenConfiguration config;

		/// <summary>
		/// Set the <see cref="ConfirmationScreenConfiguration"/> of the <see cref="ConfirmationScreen"/>.
		/// </summary>
		/// <param name="configuration">The <see cref="ConfirmationScreenConfiguration"/> that the <see cref="ConfirmationScreen"/> will use to setup.</param>
		/// <remarks>Giving a null value or leaving the parameter blank results in usage of the default configuration.</remarks>
		public void Activate(ConfirmationScreenConfiguration configuration = null)
		{
			config = configuration ?? defaultConfiguration;

			UpdateText();
			gameObject.SetActive(true);
		}
		
		private void Awake()
		{
			confirmButton.AddListener(ConfirmButtonPressed);
			cancelButton.AddListener(CancelButtonPressed);
			
			config = defaultConfiguration;
		}

		private void Start()
		{
			UpdateText();
		}

		private void CancelButtonPressed()
		{
			config.OnCancel?.Invoke();
			gameObject.SetActive(false);
		}

		private void ConfirmButtonPressed()
		{
			config.OnConfirm?.Invoke();
			gameObject.SetActive(false);
		}

		private void UpdateText()
		{
			messageText.text = config.MessageText;
			confirmButton.TextMesh.text = config.ConfirmButtonText;
			cancelButton.TextMesh.text = config.CancelButtonText;
		}
	}
}