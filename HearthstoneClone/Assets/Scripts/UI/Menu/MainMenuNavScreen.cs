using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
	/// <summary>
	/// Handles the navigation buttons in the main menu.
	/// </summary>
	public class MainMenuNavScreen : MonoBehaviour
	{
		[SerializeField] private ButtonContainer startButton;
		[SerializeField] private ButtonContainer decksButton;
		[SerializeField] private ButtonContainer exitButton;

		private bool startButtonClicked = false;
		private bool decksButtonClicked = false;

		private void OnEnable()
		{
			startButton.AddListener(OnStartButtonClicked);
			decksButton.AddListener(OnDecksButtonClicked);
			exitButton.AddListener(OnExitButtonClicked);
		}

		private void OnDisable()
		{
			startButton.RemoveListeners();
			decksButton.RemoveListeners();
			exitButton.RemoveListeners();
		}

		private void OnStartButtonClicked()
		{
			if (!startButtonClicked)
			{
				startButtonClicked = true;
				SceneManager.LoadSceneAsync("Game");
			}
		}

		private void OnDecksButtonClicked()
		{
			if (!decksButtonClicked)
			{
				decksButtonClicked = true;
				SceneManager.LoadSceneAsync("Decks");
			}
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