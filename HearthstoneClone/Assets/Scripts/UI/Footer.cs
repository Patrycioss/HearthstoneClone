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
		
		private SceneSwapper sceneSwapper;
		
		protected void Start()
		{
			sceneSwapper = GameManager.Instance.SceneSwapper;
		}
		
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
			Back();
		}

		protected virtual void OnExitButtonClicked()
		{
			Exit();
		}
		
		protected virtual void Back()
		{
			sceneSwapper.SetScene(sceneSwapper.PreviousScene);
		}

		protected virtual void Exit()
		{
			sceneSwapper.ExitGame();
		}
	}
}