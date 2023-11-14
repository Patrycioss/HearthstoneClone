using UISystem;

namespace Actions
{
	public class StopGameGameAction : GameAction
	{
		public override void Act()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;			
#else
			Application.Quit();
#endif
		}		
	}
}