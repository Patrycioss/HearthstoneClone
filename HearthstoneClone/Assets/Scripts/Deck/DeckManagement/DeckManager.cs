using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrorHandling;
using UnityEngine;
using Path = System.IO.Path;

namespace Deck.DeckManagement
{
	/// <summary>
	/// Manages the User's collection of <see cref="DeckInfo"/>.
	/// </summary>
	public class DeckManager : MonoBehaviour
	{
		[Header("Saving/Loading")]
		[SerializeField] private string saveFolderName = "UserDecks";

		[Header("Debugging")] 
		[SerializeField] private string debugSaveFolderName = "DebugDecks";
		[SerializeField] private List<DeckInfo> debugDecks;
		
		private DeckLoader deckLoader = new DeckLoader();
		private DeckSaver deckSaver = new DeckSaver();

		private string saveFolderPath;
		private string debugSaveFolderPath;
		
		private List<DeckInfo> userDecks = new List<DeckInfo>();

		/// <summary>
		/// Initializes the <see cref="DeckManager"/>.
		/// </summary>
		public void Initialize()
		{
			saveFolderPath = Path.Combine(Application.persistentDataPath, saveFolderName);
			debugSaveFolderPath = Path.Combine(Application.persistentDataPath, debugSaveFolderName);
		}
		
		/// <summary>
		/// Adds a <see cref="DeckInfo"/> to the collection of decks.
		/// <param name="deckInfo"><see cref="DeckInfo"/> to be added.</param>
		/// <param name="isDebug">Add the deck to the debug collection.</param>
		/// </summary>
		public Result AddDeck(DeckInfo deckInfo, bool isDebug = false)
		{
			Result result = new Result();
			int index = GetDeckIndex(deckInfo, isDebug);
			
			if (index == -1)
			{
				if (isDebug)
				{
					debugDecks.Add(deckInfo);
				}
				else
				{
					userDecks.Add(deckInfo);
				}
				
				result.Message += $"Added {deckInfo} to {(isDebug ? "debug" : "user")} collection.";
			}
			else
			{
				DeckInfo oldDeck;
				if (isDebug)
				{
					oldDeck = debugDecks[index];
					debugDecks[index] = deckInfo;
				}
				else
				{
					oldDeck = userDecks[index];
					userDecks[index] = deckInfo;
				}
				
				result.Message += $"Replaced {oldDeck} at {index} with {deckInfo} in the {(isDebug ? "debug" : "user")} collection.";
			}

			result.Success = true;
			return result;
		}

		/// <summary>
		/// Gets a <see cref="DeckInfo"/> from the user's collection.
		/// <param name="deckName">Name of the deck.</param>
		/// <param name="isDebug">Is the deck part of the debug collection.</param>
		/// </summary>
		public Result<DeckInfo> GetDeck(string deckName, bool isDebug = false)
		{
			Result<DeckInfo> result = new()
			{
				Value = isDebug ? 
					debugDecks.Find(info => info.Name == deckName) 
					: userDecks.Find(info => info.Name == deckName)
			};

			if (result.Value == null)
			{
				return result + $"Failed to find deck with name {deckName}";
			}

			result.Success = true;
			return result;
		}

		/// <summary>
		/// Saves a user deck from memory to disk.
		/// </summary>
		/// <param name="deckName">Name of the deck.</param>
		/// <param name="isDebug">Should the deck be saved in the debug save folder..</param>
		/// <returns>a <see cref="Result"/>.</returns>
		public async Task<Result> SaveDeck(string deckName, bool isDebug = false)
		{
			Result result = new Result();

			Result<DeckInfo> deckResult = GetDeck(deckName, isDebug);
			
			result += deckResult.Message;

			if (!deckResult)
			{
				return result;
			}

			string path;
			try
			{ 
				path = Path.Combine(saveFolderPath, deckName);
			}
			catch (Exception e)
			{
				return deckResult + $"Failed to make path combining {saveFolderPath} and {deckName}. Exception {e} occurred!";
			}
			
			Result saveResult = await deckSaver.SaveDeck(deckResult.Value, path);

			result += saveResult.Message;
			result.Success = true;
			return result;
		}

		/// <summary>
		/// Saves all user decks in memory to disk.
		/// </summary>
		/// <param name="isDebug">Should it save the debug decks.</param>
		/// <returns>A <see cref="Result"/>.</returns>
		public async Task<Result> SaveAllDecks(bool isDebug = false)
		{
			return await deckSaver.SaveDecksToDirectory(
				isDebug? debugDecks : userDecks, 
				isDebug? debugSaveFolderPath : saveFolderPath);
		}

		/// <summary>
		/// Loads all user decks from disk to memory.
		/// </summary>
		/// <param name="isDebug">Should it load the debug decks.</param>
		/// <returns>A <see cref="Result{T}"/> where T is a list of <see cref="DeckInfo"/>.</returns>
		public async Task<Result> LoadAllDecks(bool isDebug = false)
		{
			Result result = new Result();
			Result<List<DeckInfo>> loadResult = await deckLoader.LoadDecksFromDirectory(isDebug? debugSaveFolderPath : saveFolderPath);

			result += loadResult.Message;
			
			if (loadResult.Value != null)
			{
				foreach (DeckInfo deckInfo in loadResult.Value)
				{
					Result addResult = AddDeck(deckInfo, isDebug);
					result += addResult.Message;
				}
			}
			
			if (loadResult)
			{
				result.Success = true;
			}

			return result;
		}

		/// <summary>
		/// Load a deck from disk to memory.
		/// </summary>
		/// <param name="deckName">Name of the deck.</param>
		/// <param name="isDebug">Is the deck in the debug folder.</param>
		/// <returns>A <see cref="Result{T}"/> where T is a <see cref="DeckInfo"/>.</returns>
		public async Task<Result> LoadDeck(string deckName, bool isDebug = false)
		{
			Result result = new Result();

			string path;
			try
			{ 
				path = Path.Combine(isDebug? debugSaveFolderPath : saveFolderPath, deckName);
			}
			catch (Exception e)
			{
				return result + $"Failed to make path combining {saveFolderPath} and {deckName}. Exception {e} occurred!";
			}
			
			Result<DeckInfo> loadResult = await deckLoader.LoadDeck(path);
			result += loadResult.Message;

			if (loadResult)
			{
				Result addResult = AddDeck(loadResult.Value, isDebug);
				
				result += addResult.Message;
				result.Success = true;
			}

			return result;
		}

		/// <summary>
		/// Looks for the index of the <see cref="DeckInfo"/> in the collection of decks.
		/// </summary>
		/// <param name="deckInfo"><see cref="DeckInfo"/> to look for.</param>
		/// <param name="isDebug">Is it part of the debug collection.</param>
		/// <returns> -1 if not found and otherwise an index >= 0.</returns>
		private int GetDeckIndex(DeckInfo deckInfo,bool isDebug)
		{
			return isDebug ? debugDecks.IndexOf(deckInfo) : userDecks.IndexOf(deckInfo);
		}
	}
}