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


   public static GameManager Instance;
   
   private async void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      else
      {
         Destroy(this);
      }
      
      DontDestroyOnLoad(this);
      deckManager.Initialize();

      await deckManager.SaveDeck("Testie", true);
   }
}
