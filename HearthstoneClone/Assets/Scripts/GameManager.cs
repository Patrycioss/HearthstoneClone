using System.Collections.Generic;
using Deck.DeckManagement;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Manages other managers that are important throughout the whole game.
/// </summary>
public class GameManager : MonoBehaviour
{
   /// <summary>
   /// Active instance of this GameManager.
   /// </summary>
   public static GameManager Instance;
   
   /// <summary>
   /// Active <see cref="DiskManager"/>.
   /// </summary>
   public DiskManager DiskManager { get; private set; }
   
   /// <summary>
   /// Active <see cref="SceneSwapper"/>.
   /// </summary>
   public SceneSwapper SceneSwapper { get; private set; }

   /// <summary>
   /// Starting health of a player.
   /// </summary>
   public int PlayerStartHealth => playerStartHealth;

   /// <summary>
   /// Maximum mana a player can have.
   /// </summary>
   public int MaxMana => maxMana;

   /// <summary>
   /// Starting mana amount a player has.
   /// </summary>
   public int StartMana => startMana;

   /// <summary>
   /// Maximum amount of cards in hand.
   /// </summary>
   public int MaxCardsInHand => maxCardsInHand;
   
   /// <summary>
   /// Turn length in seconds.
   /// </summary>
   public int TurnLength => turnLength; 
   
   [SerializeField] private int playerStartHealth = 30;
   [SerializeField] private int maxMana = 10;
   [SerializeField] private int startMana = 1;
   [SerializeField] private int maxCardsInHand = 5;
   [Tooltip("Turn Length in Seconds"),SerializeField] private int turnLength = 20;
   
   private Dictionary<string, Transferable> transferableData;

   /// <summary>
   /// Add a <see cref="ITransferable"/> to transfer to other scenes.
   /// </summary>
   /// <param name="identifier">Name to identify the <see cref="ITransferable"/> with.</param>
   /// <param name="transferable"><see cref="ITransferable"/> to transfer.</param>
   public void AddTransferable(string identifier, Transferable transferable)
   {
      if (transferableData.ContainsKey(identifier))
      {
         transferableData.Remove(identifier);
      }
      
      transferableData.Add(identifier, transferable);
   }

   /// <summary>
   /// Get a <see cref="Transferable"/>.
   /// </summary>
   /// <param name="identifier">The identifier to get the <see cref="Transferable"/>.</param>
   /// <remarks>Removes the <see cref="Transferable"/> from the collection.</remarks>
   /// <returns>A <see cref="Transferable"/> if it can find the given identifier.</returns>
   [CanBeNull]
   public Transferable GetTransferable(string identifier)
   {
      if (transferableData.Remove(identifier, out Transferable transferable))
      {
         return transferable;
      }

      return null;
   }
   
   private void Awake()
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

      transferableData = new Dictionary<string, Transferable>();

      DiskManager = new DiskManager();
      DiskManager.Initialize();

      SceneSwapper = new SceneSwapper();
      SceneSwapper.Initialize();
   }
}
