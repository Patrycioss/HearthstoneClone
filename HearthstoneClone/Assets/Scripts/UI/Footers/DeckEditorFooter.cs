using UI.DeckEditor;
using UnityEngine;

namespace UI.Footers
{
	/// <summary>
	/// Overrides the standard footer functions to notify the user to save before leaving.
	/// </summary>
	public class DeckEditorFooter : Footer
	{
		[SerializeField] private DeckInformation deckInformation;
		
		protected override void OnBackButtonClicked()
		{
			if (deckInformation.ShouldSave)
			{	
				// Todo: show go back without saving screen.
			}
		}

		protected override void OnExitButtonClicked()
		{
			if (deckInformation.ShouldSave)
			{
				// Todo: show quit without saving screen.
			}
		}
	}
}