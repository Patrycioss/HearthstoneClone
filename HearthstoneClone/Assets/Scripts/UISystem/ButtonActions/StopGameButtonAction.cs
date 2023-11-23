#if !UNITY_EDITOR
using UnityEngine;
#endif

namespace UISystem.ButtonActions
{
	public class StopGameButtonAction : ButtonAction
	{
		protected override void OnClick()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;			
#else
			Application.Quit();
#endif
		}		
	}
}