using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	/// <summary>
	/// Handles the navigation buttons in the main menu.
	/// </summary>
	public class MainMenuNavScreen : MonoBehaviour
	{
		[SerializeField] private Button startButton;
		[SerializeField] private Button decksButton;
		[SerializeField] private Button exitButton;

		private bool startButtonClicked = false;
		private bool decksButtonClicked = false;

		private void Awake()
		{
			startButton.onClick.AddListener(OnStartButtonClicked);
			decksButton.onClick.AddListener(OnDecksButtonClicked);
			exitButton.onClick.AddListener(OnExitButtonClicked);
		}

		private void OnDisable()
		{
			startButton.onClick.RemoveAllListeners();
			decksButton.onClick.RemoveAllListeners();
			exitButton.onClick.RemoveAllListeners();
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