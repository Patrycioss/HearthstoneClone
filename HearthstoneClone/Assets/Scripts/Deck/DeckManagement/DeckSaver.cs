using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ErrorHandling;
using UnityEngine;

namespace Deck.DeckManagement
{
	/// <summary>
	/// Saves decks from memory to disk.
	/// </summary>
	public class DeckSaver
	{
		private const string TEMP_PATH_MODIFIER = "TEMP_MODIFIER";
		
		/// <summary>
		/// Save a <see cref="DeckInfo"/> to disk.
		/// </summary>
		/// <param name="deck"><see cref="DeckInfo"/> to save to disk.</param>
		/// <param name="path">Path to save the <see cref="DeckInfo"/> to.</param>
		/// <returns>Whether the saving succeeded or not.</returns>
		public async Task<Result> SaveDeck(DeckInfo deck, string path)
		{
			Result result = new Result();

			if (deck == null)
			{
				return result + $"Can't save deck that is null!";
			}

			string dataToStore = JsonUtility.ToJson(deck, true);

			if (string.IsNullOrEmpty(dataToStore))
			{
				return result + $"Failed to convert {deck.Name} to JSON!";
			}

			bool fileAlreadyExists = File.Exists(path);
			string originalPath = new string(path);

			if (fileAlreadyExists)
			{
				path += TEMP_PATH_MODIFIER;
			}

			try
			{
				await using (FileStream stream = new FileStream(path, FileMode.Create))
				{
					await using (StreamWriter writer = new StreamWriter(stream))
					{
						await writer.WriteAsync(dataToStore);
					}
				}
			}
			catch (Exception e)
			{
				return result + $"Failed to save deck to {path}. Exception {e} occurred!";
			}

			// Only delete and rename the file if saving succeeded.
			if (fileAlreadyExists)
			{
				// Try to delete original file.
				try
				{
					File.Delete(originalPath);
				}
				catch (Exception e)
				{
					return result + $"Failed to delete deck with originalPath {originalPath}. Exception {e} occurred!";
				}

				// Try to move new file to original destination.
				try
				{
					File.Move(path, originalPath);
				}
				catch (Exception e)
				{
					return result + $"Failed to move deck with path {path} to originalPath {originalPath}. Exception {e} occurred!";
				}
			}

			result += $"Saved deck {deck} to {originalPath}";
			return result;
		}

		/// <summary>
		/// Save a list of <see cref="DeckInfo"/> to disk.
		/// </summary>
		/// <param name="decks">List of info for decks to be created.</param>
		/// <param name="directoryPath">Path of the directory the decks should be saved in.</param>
		/// <param name="createDirectory">If true creates the directory if it can't find it.</param>
		public async Task<Result> SaveDecksToDirectory(List<DeckInfo> decks, string directoryPath, bool createDirectory = true)
		{
			Result result = new Result();
			
			if (string.IsNullOrEmpty(directoryPath))
			{
				return result + $"{nameof(directoryPath)} is empty!";
			}

			if (!Directory.Exists(directoryPath))
			{
				if (createDirectory)
				{
					try
					{
						Directory.CreateDirectory(directoryPath);
					}
					catch (Exception e)
					{
						return result + $"Failed to create directory with path {directoryPath}. Exception {e} occurred!";
					}

					result += $"Created directory with path {directoryPath}.";
				}
				else
				{
					return result + $"Could not find directory with path {directoryPath} and {nameof(createDirectory)} is set to false!";
				}
			}

			bool success = true;

			foreach (DeckInfo deck in decks)
			{
				if (string.IsNullOrEmpty(deck.Name))
				{
					result += $"Failed to save deck {deck} as {nameof(deck.Name)} is empty!";
					success = false;
					continue;
				}

				string path = Path.Combine(directoryPath, deck.Name);
				
				Result saveResult = await SaveDeck(deck, path);
				result += saveResult.Message;

				if (!saveResult.Success)
				{
					success = false;
				}
			}

			result.Success = success;
			
			return result;
		}
	}
}