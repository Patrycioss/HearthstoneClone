using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Deck.DeckManagement
{
	/// <summary>
	/// Saves decks from memory to disk.
	/// </summary>
	public class DeckSaver
	{
		private string folderPath;
		
		/// <summary>
		/// Initialize the deck saver.
		/// </summary>
		/// <param name="saveFolderPath">Path of the folder in which to save the decks.</param>
		public void Initialize(string saveFolderPath)
		{
			folderPath = saveFolderPath;

			if (!Directory.Exists(saveFolderPath))
			{
				Debug.Log($"{nameof(DeckSaver)} created save folder with path {saveFolderPath}");
				Directory.CreateDirectory(saveFolderPath);
			}
			else Debug.Log($"{nameof(DeckSaver)} found save folder with path {saveFolderPath}");
		}

		/// <summary>
		/// Save a <see cref="DeckInfo"/> to disk.
		/// </summary>
		/// <param name="deck"></param>
		/// <returns>Whether the saving succeeded or not.</returns>
		public Result SaveDeck(DeckInfo deck)
		{
			if (string.IsNullOrEmpty(deck.Name))
			{
				return new Result
				{
					Success = SuccessType.Failed,
					Message = new Message($"Can't save deck as name for {deck} is null or empty!"),
				};
			}

			string path = Path.Combine(folderPath, deck.Name);
			string dataToStore = JsonUtility.ToJson(deck, true);

			if (string.IsNullOrEmpty(dataToStore))
			{
				return new Result
				{
					Success = SuccessType.Failed,
					Message = new Message($"Failed to convert {deck.Name} to JSON!"),
				};
			}

			if (File.Exists(path))
			{
				File.Delete(path);
			}

			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				using (StreamWriter writer = new StreamWriter(stream))
				{
					writer.WriteAsync(dataToStore);
				}
			}

			return new Result();
		}

		/// <summary>
		/// Save a list of <see cref="DeckInfo"/> to disk.
		/// </summary>
		/// <param name="decks">List of info for decks to be created.</param>
		public Result SaveDecks(List<DeckInfo> decks)
		{
			Result result = new Result();

			foreach (DeckInfo deck in decks)
			{
				Result saveResult = SaveDeck(deck);

				if (saveResult.Success is SuccessType.Failed or SuccessType.Problem)
				{
					result.Message += saveResult.Message;

					result.Success = result.Success switch
					{
						SuccessType.Success => saveResult.Success,
						SuccessType.Problem when saveResult.Success == SuccessType.Failed => SuccessType.Failed,
						_ => result.Success
					};
				}
			}

			return result;
		}
	}
}