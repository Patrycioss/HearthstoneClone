using UnityEngine;

namespace UI.DeckEditor
{
	/// <summary>
	/// Overrides the standard footer functions to notify the user to save before leaving.
	/// </summary>
	public class DeckEditorFooter : Footer
	{
		[SerializeField] private DeckInformation deckInformation;

		private TransferableSceneData previousSceneData;
		
		protected void Start()
		{ 
			previousSceneData = GameManager.Instance.GetTransferable("PreviousScene") as TransferableSceneData;

			if (previousSceneData == null)
			{
				Debug.LogWarning($"Couldn't find previous scene data with key PreviousScene in transferable data!");
			}
		}

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