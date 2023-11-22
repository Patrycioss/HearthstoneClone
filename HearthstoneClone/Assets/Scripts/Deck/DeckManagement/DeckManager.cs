using System.Collections.Generic;
using Card;
using UnityEngine;
using Path = System.IO.Path;

namespace Deck.DeckManagement
{
	/// <summary>
	/// Manages the User's collection of <see cref="DeckInfo"/>.
	/// </summary>
	public class DeckManager
	{
		private List<DeckInfo> decks = new();

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
			
			deckSaver.SaveDeck(new DeckInfo()
			{
				Cards = new List<CardInfo>(){new ()
				{
					Image = null,
					ManaCost = 5,
					Name = "Test Card",
				}},
				Name = "Test Deck",
				Description = "Not so long test description :)",
			});
		}
		
		/// <summary>
		/// Adds a <see cref="DeckInfo"/> to the user's collection.
		/// </summary>
		public void AddDeck(DeckInfo deckInfo)
		{
			decks.Add(deckInfo);
			deckSaver.SaveDeck(deckInfo);
		}

		/// <summary>
		/// Gets a <see cref="DeckInfo"/> from the user's collection.
		/// </summary>
		public DeckInfo GetDeck(int index)
		{
			return decks[index];
		}

		private void LoadDecks()
		{
			
		}
		
	}
}