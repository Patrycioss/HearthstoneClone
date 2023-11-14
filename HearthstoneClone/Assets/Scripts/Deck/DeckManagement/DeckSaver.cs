using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Deck.DeckManagement
{
	public class DeckSaver
	{
		private string folderPath;
		
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

		public void SaveDeck(DeckInfo deck)
		{
			if (string.IsNullOrEmpty(deck.Name))
			{
				Debug.LogError($"Can't save deck as name is null or empty!");
				return;
			}

			string path = Path.Combine(folderPath, deck.Name);
			string dataToStore = JsonUtility.ToJson(deck, true);

			if (string.IsNullOrEmpty(dataToStore))
			{
				Debug.LogError($"Something went wrong with converting {deck.Name} to JSON!");
				return;
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
		}

		public void SaveDecks(List<DeckInfo> decks)
		{
			foreach (DeckInfo deck in decks)
			{
				SaveDeck(deck);
			}
		}
	}
}