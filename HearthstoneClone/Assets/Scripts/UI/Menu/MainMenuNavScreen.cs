using UnityEngine;

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

		private SceneSwapper sceneSwapper;

		private void Start()
		{
			sceneSwapper = GameManager.Instance.SceneSwapper;
		}

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
				sceneSwapper.SetScene(SceneSwapper.Scene.Game);
			}
		}

		private void OnDecksButtonClicked()
		{
			if (!decksButtonClicked)
			{
				decksButtonClicked = true;
				sceneSwapper.SetScene(SceneSwapper.Scene.DeckView);
			}
		}

		private void OnExitButtonClicked()
		{
			sceneSwapper.ExitGame();
		}
	}
}