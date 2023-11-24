using Deck;
using Deck.DeckManagement;
using UnityEngine;

/// <summary>
/// Manages information between scenes.
/// </summary>
public class Core : MonoBehaviour
{
   /// <summary>
   /// Active <see cref="DeckManager"/>
   /// </summary>
   public DeckManager DeckManager { get; set; }

   [SerializeField] private string saveFolderName = "UserDecks";

   [SerializeField] private DeckInfo testDeck;
   
   
   private void Awake()
   {
      DontDestroyOnLoad(this);

      DeckManager = new DeckManager();
      DeckManager.Initialize(saveFolderName);
      
      DeckManager.AddUserDeck(testDeck);
   }
}
