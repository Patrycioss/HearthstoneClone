using UnityEngine;
using UnityEngine.UI;

namespace UI.Footers
{
	/// <summary>
	/// Handles the base navigation in the footer.
	/// </summary>
	public class Footer : MonoBehaviour
	{
		[SerializeField] private Button backButton;
		[SerializeField] private Button exitButton;
		
		protected virtual void Awake()
		{
			backButton.onClick.AddListener(OnBackButtonClicked);
			exitButton.onClick.AddListener(OnExitButtonClicked);
		}

		protected virtual void OnDisable()
		{
			backButton.onClick.RemoveAllListeners();
			exitButton.onClick.RemoveAllListeners();
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