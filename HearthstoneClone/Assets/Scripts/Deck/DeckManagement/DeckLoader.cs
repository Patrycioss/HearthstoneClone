using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ErrorHandling;
using UnityEngine;

namespace Deck.DeckManagement
{
	/// <summary>
	/// Loads desk from disk into memory.
	/// </summary>
	public class DeckLoader
	{
		/// <summary>
		/// Loads all user decks from disk into memory.
		/// </summary>
		public async Task<Result<List<DeckInfo>>> LoadDecksFromDirectory(string directoryPath)
		{
			Result<List<DeckInfo>> result = new();
			
			// Make sure folderPath is not an empty string.
			if (string.IsNullOrEmpty(directoryPath))
			{
				result.Message += $"{nameof(directoryPath)} is an empty string!";
				return result;
			}

			// Make sure the directory exists.
			if (!Directory.Exists(directoryPath))
			{
				result.Message += $"Path {directoryPath} is not a valid directory!";
				return result;
			}

			return await LoadDecks( Directory.GetFiles(directoryPath));
		}

		/// <summary>
		/// Load a list of <see cref="DeckInfo"/> from disk to memory.
		/// </summary>
		/// <param name="deckPaths">Paths of the decks to load.</param>
		/// <returns>A <see cref="Result{T}"/> where T is a list of <see cref="DeckInfo"/>.</returns>
		public async Task<Result<List<DeckInfo>>> LoadDecks(IEnumerable<string> deckPaths)
		{
			Result<List<DeckInfo>> result = new Result<List<DeckInfo>>
			{
				Value = new List<DeckInfo>()
			};

			foreach (string deckPath in deckPaths)
			{
				Result<DeckInfo> loadResult = await LoadDeck(deckPath);
				result += loadResult.Message;

				if (!loadResult)
				{
					result.Success = false;
				}
				else
				{
					result.Value.Add(loadResult.Value);
				}
			}

			return result;
		}

		/// <summary>
		/// Load a <see cref="DeckInfo"/> from disk to memory.
		/// </summary>
		/// <param name="deckPath">Name of the deck to load.</param>
		/// <returns>An object containing the deck and whether it succeeded retrieving it.</returns>
		public async Task<Result<DeckInfo>> LoadDeck(string deckPath)
		{
			Result<DeckInfo> result = new Result<DeckInfo>();
			
			// Make sure the provided name isn't invalid.
			if (string.IsNullOrEmpty(deckPath))
			{
				return result + $"Can't load deck as {nameof(deckPath)} is null or empty!";
			}

			// Make sure the file exists.
			if (!File.Exists(deckPath))
			{
				return result + $"Can't load deck as path {deckPath} does not exist!";
			}

			// Try to load the contents from the file into the buffer.
			char[] buffer;
			
			StringBuilder builder = new StringBuilder();
			
			try
			{
				using (StreamReader reader = File.OpenText(deckPath))
				{
					buffer = new char[reader.BaseStream.Length];
					await reader.ReadAsync(buffer, 0, (int)reader.BaseStream.Length);
				}
			}
			catch (Exception e)
			{
				return result + $"Failed to load deck from file with path {deckPath}. Exception '{e}' occurred!";
			}

			// Convert buffer to a string and make sure conversion goes well.
			foreach (char c in buffer)
			{
				builder.Append(c);
			}
			
			string readData = builder.ToString();
			
			if (string.IsNullOrEmpty(readData))
			{
				return result + $"Read text from file with path {deckPath} is empty!";
			}

			// Convert json string to DeckInfo and make sure conversion goes well.
			DeckInfo deckInfo;
			try
			{
				deckInfo = JsonUtility.FromJson<DeckInfo>(readData);
			}
			catch (Exception e)
			{
				return result + $"Failed to convert {readData} to {nameof(DeckInfo)}. Exception {e} occurred!";
			}

			if (deckInfo == null)
			{
				return result + $"Failed to convert {readData} to {nameof(DeckInfo)}!";
			}

			result.Success = true;
			result.Value = deckInfo;
			result += $"Loaded {deckInfo} successfully!";
			return result;
		}
	}
}