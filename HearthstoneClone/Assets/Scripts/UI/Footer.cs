using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	/// <summary>
	/// Handles the navigation in the footer.
	/// </summary>
	public class Footer : MonoBehaviour
	{
		[SerializeField] private Button backButton;
		[SerializeField] private Button exitButton;
		
		private void Awake()
		{
			backButton.onClick.AddListener(OnBackButtonClicked);
			exitButton.onClick.AddListener(OnExitButtonClicked);
		}

		private void OnDisable()
		{
			backButton.onClick.RemoveAllListeners();
			exitButton.onClick.RemoveAllListeners();
		}
		
		
		private void OnBackButtonClicked()
		{
			//TODO: Implement	
		}

		private void OnExitButtonClicked()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;			
#else
			Application.Quit();
#endif
		}
	}
}