﻿using UnityEngine;

namespace Extensions
{
	/// <summary>
	/// Helper functions to log debug information with timestamps.
	/// </summary>
	public class TimedLogger
	{
		public bool Enabled { get; set; } = true;
		
		public void Log(string message)
		{
			if (Enabled)
			{
				Debug.Log($"{message} at {Time.realtimeSinceStartup}");
			}
		}
		
		public void LogWarning(string message)
		{
			if (Enabled)
			{
				Debug.LogWarning($"{message} at {Time.realtimeSinceStartup}");
			}
		}
		
		public void LogError(string message)
		{
			if (Enabled)
			{
				Debug.LogError($"{message} at {Time.realtimeSinceStartup}");
			}
		}
	}
}