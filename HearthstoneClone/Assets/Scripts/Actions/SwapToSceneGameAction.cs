using External;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Actions
{
	public class SwapToSceneGameAction : GameAction
	{
		[SerializeField] private SceneField sceneField;
		
		public override void Act()
		{
			SceneManager.LoadSceneAsync(sceneField);
		}
	}
}