using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace UI.Generic
{
	/// <summary>
	/// Script that steers the UI for a generic message.
	/// </summary>
	public class Message : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI message;
		[SerializeField] private ButtonContainer continueButton;

		private MessageConfiguration config;

		/// <summary>
		/// Activate the generic message.
		/// </summary>
		/// <param name="messageConfiguration">The <see cref="MessageConfiguration"/> with which to activate it.</param>
		public void Activate([NotNull] MessageConfiguration messageConfiguration)
		{
			config = messageConfiguration;
			
			message.text = messageConfiguration.MessageText;
			continueButton.TextMesh.text = messageConfiguration.ButtonText;
			gameObject.SetActive(true);
		}
		
		private void OnEnable()
		{
			continueButton.AddListener(OnContinueButtonClicked);
		}

		private void OnDisable()
		{
			continueButton.RemoveListeners();
		}

		private void OnContinueButtonClicked()
		{
			gameObject.SetActive(false);
			config.OnContinue?.Invoke();
		}
	}
}