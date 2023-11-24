using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Path = System.IO.Path;

namespace Deck.DeckManagement
{
	/// <summary>
	/// Manages the User's collection of <see cref="DeckInfo"/>.
	/// </summary>
	public class DeckManager : MonoBehaviour
	{
		[SerializeField] private List<DeckInfo> testDecks;
		
		private List<DeckInfo> userDecks = new();

		private DeckLoader deckLoader = new();
		private DeckSaver deckSaver = new();

		/// <summary>
		/// Initializes the <see cref="DeckManager"/>.
		/// </summary>
		public void Initialize(string saveFolderName)
		{
			string saveFolderPath = Path.Combine(Application.persistentDataPath, saveFolderName); 
			deckLoader.Initialize(saveFolderPath);
			deckSaver.Initialize(saveFolderPath);
		}
		
		/// <summary>
		/// Adds a <see cref="DeckInfo"/> to the user's collection.
		/// </summary>
		public void AddUserDeck(DeckInfo deckInfo)
		{
			userDecks.Add(deckInfo);
			deckSaver.SaveDeck(deckInfo);
		}

		/// <summary>
		/// Gets a <see cref="DeckInfo"/> from the user's collection.
		/// </summary>
		[Pure]
		public Result<DeckInfo> GetUserDeck(string deckName)
		{
			Result<DeckInfo> result = new()
			{
				Value = userDecks.Find(info => info.Name == deckName),
			};

			if (result.Value == null)
			{
				result.Message += $"Failed to find deck with name {deckName}";
				result.Success = SuccessType.Failed;
			}

			return result;
		}

		/// <summary>
		/// Saves a user deck from memory to disk.
		/// </summary>
		/// <param name="deckName">Name of the deck.</param>
		/// <returns>Whether the deck saved correctly.</returns>
		public Result SaveUserDeck(string deckName)
		{
			Result<DeckInfo> deckResult = GetUserDeck(deckName);

			Result result = new()
			{
				Success = deckResult.Success,
				Message = deckResult.Message
			};

			if (result.Success == SuccessType.Failed)
			{
				return result;
			}
			
			return deckSaver.SaveDeck(deckResult.Value);
		}

		/// <summary>
		/// Saves all user decks in memory to disk.
		/// </summary>
		public Result SaveAllUserDecks()
		{
			Result result = new Result();
			
			foreach (DeckInfo deckInfo in userDecks)
			{
				Result saveResult = deckSaver.SaveDeck(deckInfo);
				
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

		/// <summary>
		/// Loads all user decks from disk into memory.
		/// </summary>
		public void LoadUserDecks()
		{
			
		}
		
	}
}