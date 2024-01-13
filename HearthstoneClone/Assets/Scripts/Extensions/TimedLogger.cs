using UnityEngine;

namespace Extensions
{
	/// <summary>
	/// Helper functions to log debug information with timestamps.
	/// </summary>
	public static class TimedLogger
	{
		public static void Log(string message)
		{
			Debug.Log($"{message} at {Time.realtimeSinceStartup}");
		}
		
		public static void LogWarning(string message)
		{
			Debug.LogWarning($"{message} at {Time.realtimeSinceStartup}");
		}
		
		public static void LogError(string message)
		{
			Debug.LogError($"{message} at {Time.realtimeSinceStartup}");
		}
	}
}