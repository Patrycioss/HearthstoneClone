using UnityEngine;

namespace UI
{
	/// <summary>
	/// Handles the base navigation in the footer.
	/// </summary>
	public class Footer : MonoBehaviour
	{
		[SerializeField] protected ButtonContainer backButton;
		[SerializeField] protected ButtonContainer exitButton;
		
		protected virtual void OnEnable()
		{
			backButton.AddListener(OnBackButtonClicked);
			exitButton.AddListener(OnExitButtonClicked);
		}

		protected virtual void OnDisable()
		{
			backButton.RemoveListeners();
			exitButton.RemoveListeners();
		}
		
		protected virtual void OnBackButtonClicked()
		{
			//TODO: Implement	
		}

		protected virtual void OnExitButtonClicked()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;			
#else
			Application.Quit();
#endif
		}
	}
}