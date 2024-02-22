using CardComposition;
using Game;
using Settings;
using StateStuff;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PhysicalCards
{
    /// <summary>
    /// Physical card in the game.
    /// </summary>
    [RequireComponent(typeof(PhysicalCardVisuals))]
    public class PhysicalCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Can the card not be moved?
        /// </summary>
        public bool IsLocked { get; set; } = false;

        /// <summary>
        /// Determines whether the card can be used on the board, only applies to minions.
        /// </summary>
        public bool IsAwake => roundsActive > 0;
        
        /// <summary>
        /// Controls the active data of the physical card.
        /// </summary>
        public ActiveCardController CardController { get; private set; }

        /// <summary>
        /// Controls the visuals of the physical card.
        /// </summary>
        public PhysicalCardVisuals Visuals { get; private set; }

        /// <summary>
        /// <see cref="CardInfo"/> associated with this physical card.
        /// </summary>
        public CardInfo CardInfo { get; private set; }
        
        /// <summary>
        /// Is the mouse hovering over the cared?
        /// </summary>
        public bool IsHoveringOver { get; private set; } = false;
        
        /// <summary>
        /// Has the card been played?
        /// </summary>
        public bool HasBeenPlayed { get; private set; }= false;
        
        /// <summary>
        /// Owner of the card.
        /// </summary>
        public Player Owner { get; private set; }
        
        [SerializeField] private LogSettings logSettings;

        private StateMachine stateMachine;
        private CardBehaviourController cardBehaviourController;
        private bool isSelected = false;
        
        /// <summary>
        /// Amount of rounds active.
        /// <remarks>A round is a </remarks>
        /// </summary>
        private int roundsActive;
        
        /// <summary>
        /// Initialize the physical card.
        /// </summary>
        public void Initialize(CardInfo cardInfo, Player player, Transform movingContainer)
        {
            CardInfo = cardInfo;
            Owner = player;

            Visuals.Initialize(CardInfo.ImageReference);

            CardController = new ActiveCardController(CardInfo, Visuals);
            cardBehaviourController = new CardBehaviourController(cardInfo.CardBehaviour, transform);
            
            PrepareStateMachine(player, movingContainer);
        }

        /// <summary>
        /// Play the card.
        /// </summary>
        public void Play()
        {
            if (HasBeenPlayed)
            {
                return;
            }

            Owner.TryRemoveCard(this);
            HasBeenPlayed = true;
            cardBehaviourController.CallOnPlay();
        }

        /// <summary>
        /// Add a round to the internal rounds counter.
        /// </summary>
        public void CountRound()
        {
            roundsActive++;
        }

        /// <summary>
        /// Called when the mouse enters the card.
        /// </summary>
        /// <param name="eventData">Data associated with the event.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHoveringOver = true;
            PlayManager.Instance.Board.CardOfInterest = this;
            
            if (stateMachine.ActiveState is not null or HeldState)
            {
                stateMachine.SetState(new InspectingState());
            }
        }
        
        /// <summary>
        /// Select the card.
        /// <remarks>Only works when the card <see cref="HasBeenPlayed"/>.</remarks>
        /// </summary>
        public void Select()
        {
            if (HasBeenPlayed)
            {
                isSelected = true;
                cardBehaviourController.CallOnSelect();
            }
        }

        /// <summary>
        /// Deselect the card.
        /// <remarks>Only works when the card <see cref="HasBeenPlayed"/> and is currently selected.</remarks>
        /// </summary>
        public void Deselect()
        {
            if (isSelected && HasBeenPlayed)
            {
                isSelected = false;
                cardBehaviourController.CallOnDeselect();
            }
        }

        /// <summary>
        /// Called when the mouse exits the card.
        /// </summary>
        /// <param name="eventData">Data associated with the event.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            IsHoveringOver = false;
            if (PlayManager.Instance.Board.CardOfInterest != this)
            {
                PlayManager.Instance.Board.CardOfInterest = null;
            }
        }

        private void Awake()
        {
            Visuals = GetComponent<PhysicalCardVisuals>();
        }

        private void Update()
        {
            stateMachine?.Update();

            if (HasBeenPlayed)
            {
                cardBehaviourController.CallUpdate();
            }
        }

        private void PrepareStateMachine(Player player, Transform movingContainer)
        {
            stateMachine = new StateMachine(logSettings.LogStateMachine);
            stateMachine.AddReference("Card", this);
            stateMachine.AddReference("MovingContainer", movingContainer);
            stateMachine.AddReference("Player", player);
            
            stateMachine.SetState(new HeldState());
        }
    }
}

