using UnityEngine;

namespace Settings
{
	[CreateAssetMenu(fileName = "LogSettings", menuName = "Settings/LogSettings", order = 0)]
	public class LogSettings : ScriptableObject
	{
		public bool LogStateMachine = true;

		public bool LogBehaviours = true;
	}
}