using CustomEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UISystem.ButtonActions
{
	public class SwapToSceneGameAction : ButtonAction
	{
		[SerializeField] private SceneField sceneField;
		
		private void OnClick()
		{
			SceneManager.LoadSceneAsync(sceneField);
		}
	}
}