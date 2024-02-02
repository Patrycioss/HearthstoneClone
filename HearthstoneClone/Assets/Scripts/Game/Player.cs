using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardManagement.CardComposition;
using CardManagement.CardComposition.Behaviours;
using CardManagement.Physical;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.Serialization;
using Utils;

namespace Game
{
	/// <summary>
	/// Object containing all the information of a character in the game.
	/// </summary>
	public class Player : MonoBehaviour
	{
		/// <summary>
		/// Can cards be moved?
		/// </summary>
		public bool IsLocked
		{
			get => isLocked;
			set
			{
				isLocked = value;

				Debug.Log($"{physicalCards.Count}");

				foreach (PhysicalCard card in physicalCards)
				{
					card.IsLocked = isLocked;
				}
			}
		}

		/// <summary>
		/// Name of the player.
		/// </summary>
		public string PlayerName { get; private set; }

		/// <summary>
		/// Turn object of the player.
		/// </summary>
		public Turn Turn { get; private set; }

		/// <summary>
		/// Health of the player.
		/// </summary>
		public ResourceContainer Health { get; private set; }

		/// <summary>
		/// Mana of the player.
		/// </summary>
		public ResourceContainer Mana { get; private set; }

		/// <summary>
		/// Deck that the player uses.
		/// </summary>
		public PlayerDeck PlayerDeck { get; private set; }

		/// <summary>
		/// Hand of the player.
		/// </summary>
		public Transform Hand => hand;

		/// <summary>
		/// Mana bar of the player.
		/// </summary>
		public ManaBar ManaBar { get; private set; }

		/// <summary>
		/// Active target behaviour.
		/// </summary>
		public TargetBehaviour ActiveTargetBehaviour { get; set; }

		[SerializeField] private ManaBar manaBar;
		[SerializeField] private Turn turn;
		[SerializeField] private AssetLabelReference cardPrefabLabel;
		[FormerlySerializedAs("intermediateContainer")] [SerializeField] private Transform movingContainer;
		[SerializeField] private Transform hand;
		[SerializeField] private Board board;
		
		private List<PhysicalCard> physicalCards = new List<PhysicalCard>();
		private int maxCardAmount;
		private bool isLocked;

		/// <summary>
		/// Construct a new player with a set of cards in the deck.
		/// </summary>
		/// <param name="playerName">Name of the player.</param>
		/// <param name="cards">The cards the player has.</param>
		public void Initialize(string playerName, IEnumerable<CardInfo> cards)
		{
			PlayerName = playerName;
			
			maxCardAmount = GameManager.Instance.MaxCardsInHand;
			
			Turn = turn;
			ManaBar = manaBar;
			Mana = new ResourceContainer(GameManager.Instance.StartMana);
			
			Mana.OnChanged += OnManaChanged;
			Mana.SetMaximum(5);
			
			Health = new ResourceContainer(GameManager.Instance.PlayerStartHealth);
			PlayerDeck = new PlayerDeck(CollectionUtils.RandomizeList(cards.ToList()));
		}

		/// <summary>
		/// Draw cards from <see cref="PlayerDeck"/> and put them in their hand.
		/// </summary>
		/// <param name="amount">The amount of cards.</param>
		public async Task DrawCard(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				if (PlayerDeck.GetCard(out CardInfo card))
				{
					Debug.Log($"[{PlayerName}]: Drawing card: {card.CardName}");

					if (physicalCards.Count < maxCardAmount)
					{
						await SpawnCard(card);
					}
				}
				else
				{
					Debug.Log($"[{PlayerName}]: No more cards to draw!");
				}
			}
		}
		
		private async Task SpawnCard(CardInfo card)
		{
			GameObject cardObject = await ResourceUtils.InstantiateFromLabel(cardPrefabLabel,
				new InstantiationParameters(hand.transform, false));

			if (cardObject.TryGetComponent(out PhysicalCard physicalCard))
			{
				physicalCard.Initialize(card, this, movingContainer);
				
				physicalCard.Visuals.Flip();
				
				physicalCards.Add(physicalCard);
			}
		}

		private void OnDestroy()
		{
			Mana.OnChanged -= OnManaChanged;
		}

		private void OnManaChanged()
		{
			ManaBar.SetManaText(Mana.Total);
		}
	}
}