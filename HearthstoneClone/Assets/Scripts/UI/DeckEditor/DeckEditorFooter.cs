using UI.Generic;
using UnityEngine;

namespace UI.DeckEditor
{
	/// <summary>
	/// Overrides the standard footer functions to notify the user to save before leaving.
	/// </summary>
	public class DeckEditorFooter : Footer
	{
		[SerializeField] private DeckInformation deckInformation;
		[SerializeField] private ConfirmationScreen confirmationScreen;

		protected override void OnBackButtonClicked()
		{
			if (deckInformation.ShouldSave)
			{	
				confirmationScreen.Activate(new ConfirmationScreenConfiguration()
				{
					MessageText = $"Are you sure you wish to exit without saving?",
					OnConfirm = Back
				});
			}
			else Back();
		}

		protected override void OnExitButtonClicked()
		{
			if (deckInformation.ShouldSave)
			{
				confirmationScreen.Activate(new ConfirmationScreenConfiguration()
				{
					MessageText = $"Are you sure you wish to exit without saving?",
					OnConfirm = Exit
				});
			}
			else Exit();
		}
	}
}