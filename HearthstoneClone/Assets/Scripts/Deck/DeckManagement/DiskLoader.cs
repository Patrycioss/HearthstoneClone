using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ErrorHandling;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Deck.DeckManagement
{
	/// <summary>
	/// Loads an object from disk into memory.
	/// </summary>
	public class DiskLoader
	{
		/// <summary>
		/// Load a <see cref="Savable"/> from disk to memory based on path.
		/// </summary>
		/// <param name="path">Path of the <see cref="Savable"/> to load.</param>
		/// <returns>An object containing the <see cref="Savable"/> and whether it succeeded retrieving it.</returns>
		public async Task<Result<T>> LoadFromPath<T>([NotNull] string path) where T : Savable
		{
			Result<T> result = new Result<T>();
			
			// Make sure the provided path isn't empty.
			if (string.IsNullOrEmpty(path))
			{
				return result + $"Can't load savable as given path is empty!";
			}

			// Make sure the file exists.
			if (!File.Exists(path))
			{
				return result + $"Can't load savable as path {path} does not exist!";
			}

			// Try to load the contents from the file into the buffer.
			char[] buffer;
			
			StringBuilder builder = new StringBuilder();
			
			try
			{
				using (StreamReader reader = File.OpenText(path))
				{
					buffer = new char[reader.BaseStream.Length];
					await reader.ReadAsync(buffer, 0, (int)reader.BaseStream.Length);
				}
			}
			catch (Exception e)
			{
				return result + $"Failed to load savable from file with path {path}. Exception '{e}' occurred!";
			}

			// Convert buffer to a string and make sure conversion goes well.
			foreach (char c in buffer)
			{
				builder.Append(c);
			}
			
			string readData = builder.ToString();
			
			if (string.IsNullOrEmpty(readData))
			{
				return result + $"Read text from file with path {path} is empty!";
			}

			// Convert json string to Savable type T and make sure conversion goes well.
			T savable;
			try
			{
				savable = JsonConvert.DeserializeObject<T>(readData);
			}
			catch (Exception e)
			{
				return result + $"Failed to convert {readData} to {nameof(Savable)}. Exception {e} occurred!";
			}

			if (savable == null)
			{
				return result + $"Failed to convert {readData} to {nameof(Savable)}!";
			}

			result.Success = true;
			result.Value = savable;
			result += $"Loaded {savable} successfully!";
			return result;
		}
	}
}