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

		private void Awake()
		{
			if (startButton)
			{
				startButton.onClick.AddListener(OnStartButtonClicked);
			}

			if (decksButton)
			{
				decksButton.onClick.AddListener(OnDecksButtonClicked);
			}

			if (exitButton)
			{
				exitButton.onClick.AddListener(OnExitButtonClicked);
			}
		}

		private void OnDisable()
		{
			if (startButton)
			{
				startButton.onClick.RemoveListener(OnStartButtonClicked);
			}
			
			if (decksButton)
			{
				decksButton.onClick.RemoveListener(OnDecksButtonClicked);
			}

			if (exitButton)
			{
				exitButton.onClick.RemoveListener(OnExitButtonClicked);
			}
		}

		private void OnStartButtonClicked()
		{
			SceneManager.LoadSceneAsync("Game");
		}

		private void OnDecksButtonClicked()
		{
			SceneManager.LoadSceneAsync("Decks");
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