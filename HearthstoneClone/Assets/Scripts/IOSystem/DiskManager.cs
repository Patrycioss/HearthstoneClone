﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ErrorHandling;
using JetBrains.Annotations;
using UnityEngine;

namespace IOSystem
{
	/// <summary>
	/// Generic IO for saving and loading a <see cref="Savable"/> which uses <see cref="DiskSaver"/> and <see cref="DiskLoader"/>.
	/// </summary>
	public class DiskManager
	{
		/// <summary>
		/// Initializes this DiskManager.
		/// </summary>
		/// <remarks>Should only be called once.</remarks>
		public static void Initialize()
		{			
			PerformDirectoryChecks();
		}

		/// <summary>
		/// Try to remove a <see cref="Savable"/> from disk.
		/// </summary>
		/// <param name="savable">A <see cref="Savable"/> to delete.</param>
		public static void RemoveFromDisk([NotNull] Savable savable)
		{
			Result result = new Result();

			bool failed = false;
			
			Result<string> pathResult = savable.GetPath();
			if (pathResult)
			{
				try
				{
					if (File.Exists(pathResult.Value!))
					{
						File.Delete(pathResult.Value!);
					}
				}
				catch (Exception e)
				{
					result += $"Failed deleting file with path {pathResult.Value} because of exception {e}!";
					failed = true;
				}
			}

			if (!failed)
			{
				result += $"Succeeded in deleting file with path {pathResult.Value}.";
			}

			Debug.Log(result);
		}
		
		/// <summary>
		/// Save a <see cref="Savable"/> to disk using <see cref="DiskSaver"/>.
		/// </summary>
		/// <param name="savable"><see cref="Savable"/> to save.</param>
		public static async Task SaveToDisk([NotNull] Savable savable)
		{
			Result result = await DiskSaver.Save(savable);
			Debug.Log(result);
		}

		/// <summary>
		/// Save multiple <see cref="Savable"/> to disk using <see cref="DiskSaver"/>.
		/// </summary>
		/// <param name="savableList">A collection of <see cref="Savable"/>.</param>
		public static async Task SaveToDisk([NotNull] IEnumerable<Savable> savableList)
		{
			foreach (Savable savable in savableList)
			{
				await SaveToDisk(savable);
			}
		}

		/// <summary>
		/// Load a <see cref="Savable"/>.
		/// </summary>
		/// <param name="path">The path to load the <see cref="Savable"/> from.</param>
		/// <typeparam name="T">Type of <see cref="Savable"/> to load.</typeparam>
		/// <returns>The loaded <see cref="Savable"/>.</returns>
		[CanBeNull]
		public static async Task<T> LoadFromDisk<T>([NotNull] string path) where T : Savable
		{
			Result<T> result = await DiskLoader.LoadFromPath<T>(path);
			Debug.Log(result);
			return result.Value;
		}

		/// <summary>
		/// Load all <see cref="Savable"/> from a <see cref="SaveDirectory"/>.
		/// </summary>
		/// <param name="saveDirectory">The <see cref="SaveDirectory"/> to load the <see cref="Savable"/> from.</param>
		/// <typeparam name="T">Type of <see cref="Savable"/> to load.</typeparam>
		/// <returns>A <see cref="ICollection{T}"/> where T is a <see cref="Savable"/>.</returns>
		public static async Task<List<T>> LoadFromDisk<T>(SaveDirectory saveDirectory) where T : Savable
		{
			Result<List<T>> result = new Result<List<T>>()
			{
				Value = new List<T>()
			};

			string directoryPath = string.Empty;
			try
			{
				if (!saveDirectory.ToString().Equals("None"))
				{
					directoryPath = Path.Combine(Application.persistentDataPath, saveDirectory.ToString());
				}
				else
				{
					directoryPath = Application.persistentDataPath;
				}
			}
			catch (Exception e)
			{
				result += $"Failed to make path for {saveDirectory.ToString()}. Exception {e} occurred!";
			}

			if (!string.IsNullOrEmpty(directoryPath))
			{
				foreach (string filePath in Directory.GetFiles(directoryPath))
				{
					Result<T> loadResult = await DiskLoader.LoadFromPath<T>(filePath);
					result += loadResult.Message;

					if (loadResult.Value != null)
					{
						result.Value!.Add(loadResult.Value);
					}
				}
			}
			
			Debug.Log(result);
			return result.Value;
		}

		/// <summary>
		/// Check if the directories defined in <see cref="SaveDirectory"/> exist
		/// so the individual calls don't have to check for that.
		/// </summary>
		private static void PerformDirectoryChecks()
		{
			Result result = new Result();
			
			foreach (string saveDirectory in Enum.GetNames(typeof(SaveDirectory)))
			{
				if (saveDirectory.Equals("None"))
				{
					continue;
				}
				
				string path;
				try
				{
					path = Path.Combine(Application.persistentDataPath, saveDirectory);
				}
				catch (Exception e)
				{
					result += $"Failed to make path for {saveDirectory}. Exception {e} occurred!";
					continue;
				}

				if (!Directory.Exists(path))
				{
					try
					{
						Directory.CreateDirectory(path);
					}
					catch (Exception e)
					{
						result += $"Failed to create directory with path {path}. Exception {e} occurred!";
						continue;
					}

					result += $"Created directory with path {path}.";
				}
			}

			if (!string.IsNullOrEmpty(result.Message.ToString()))
			{
				Debug.Log(result);
			}
		}
	}
}