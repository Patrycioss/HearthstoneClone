using Deck.DeckManagement;
using UnityEngine;

/// <summary>
/// Manages information between scenes.
/// </summary>
public class GameManager : MonoBehaviour
{
   /// <summary>
   /// Active <see cref="DeckManager"/>.
   /// </summary>
   public DeckManager DeckManager => deckManager;

   [Header("Managers")]
   [SerializeField] private DeckManager deckManager;
   
   
   private async void Awake()
   {
      DontDestroyOnLoad(this);
      deckManager.Initialize();

      await deckManager.SaveDeck("Testie", true);
   }
}
