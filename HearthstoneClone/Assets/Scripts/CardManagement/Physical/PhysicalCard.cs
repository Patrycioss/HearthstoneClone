using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardManagement.CardComposition;
using CardManagement.CardComposition.Behaviours;
using CustomLogging;
using Game;
using Settings;
using StateStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using Utils;

namespace CardManagement.Physical
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
        public bool IsAwake { get; set; } = false;
        
        /// <summary>
        /// Controls the active data of the physical card.
        /// </summary>
        public ActiveCardController Controller { get; private set; }

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
        
        [SerializeField] private LogSettings logSettings;

        private StateMachine stateMachine;
        private List<CardBehaviour> behaviours = new List<CardBehaviour>();

        private bool initialized = false;
        
        /// <summary>
        /// Initialize the physical card.
        /// </summary>
        public async void Initialize(CardInfo cardInfo, Player player, Transform movingContainer, Board board)
        {
            CardInfo = cardInfo;

            Visuals.Initialize(CardInfo.CardName, CardInfo.Description, CardInfo.ImageReference);

            Controller = new ActiveCardController(CardInfo, Visuals);
            
            PrepareStateMachine(player, board, movingContainer);
            await InitializeBehaviours(board);
            initialized = true;
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
            
            HasBeenPlayed = true;

            foreach (CardBehaviour behaviour in behaviours)
            {
                behaviour.OnPlay();
            }
        }

        /// <summary>
        /// Called when the mouse enters the card.
        /// </summary>
        /// <param name="eventData">Data associated with the event.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHoveringOver = true;
            
            if (stateMachine.ActiveState is not null or HeldState)
            {
                stateMachine.SetState(new InspectingState());
            }
        }
        
        /// <summary>
        /// Select the card.
        /// <remarks>Only works when the card <see cref="HasBeenPlayed"/> and not <see cref="IsAwake"/>.</remarks>
        /// </summary>
        public void Select()
        {
            behaviours.ForEach(behaviour => behaviour.OnSelect());
        }

        /// <summary>
        /// Deselect the card.
        /// <remarks>Only works when the card <see cref="HasBeenPlayed"/>.</remarks>
        /// </summary>
        public void Deselect()
        {
            behaviours.ForEach(behaviour => behaviour.OnDeselect());
        }

        /// <summary>
        /// Called when the mouse exits the card.
        /// </summary>
        /// <param name="eventData">Data associated with the event.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            IsHoveringOver = false;
        }

        private void Awake()
        {
            Visuals = GetComponent<PhysicalCardVisuals>();
        }

        private void Update()
        {
            if (!initialized)
            {
                return;
            }
            
            stateMachine?.Update();

            if (HasBeenPlayed)
            {
                foreach (CardBehaviour behaviour in behaviours)
                {
                    behaviour.Update();
                }
            }
        }
        
        private async Task InitializeBehaviours(Board board)
        {
            foreach (AssetReference reference in CardInfo.CardBehaviourReferences)
            {
                GameObject prefab = await ResourceUtils.LoadAddressableFromReference<GameObject>(reference);
                GameObject spawned = Instantiate(prefab, transform);
                
                if (spawned.TryGetComponent(out CardBehaviour behaviour))
                {
                    behaviours.Add(behaviour);
                }
            }
            
            foreach (CardBehaviour behaviour in behaviours)
            {
                FieldInfo cardField = behaviour.GetType()
                    .GetField("Card", BindingFlags.Instance | BindingFlags.NonPublic);

                if (cardField != null)
                {
                    cardField.SetValue(behaviour, this);
                }
                
                FieldInfo loggerField = behaviour.GetType()
                    .GetField("Logger", BindingFlags.Instance | BindingFlags.NonPublic);

                if (loggerField != null)
                {
                    loggerField.SetValue(behaviour, new TimedLogger {Enabled = logSettings.LogBehaviours});
                }
                
                FieldInfo containerField = behaviour.GetType()
                    .GetField("Container", BindingFlags.Instance | BindingFlags.NonPublic);

                if (containerField != null)
                {
                    containerField.SetValue(behaviour, board.DrawingContainer);
                }
            }
        }

        private void PrepareStateMachine(Player player, Board board, Transform movingContainer)
        {
            stateMachine = new StateMachine(logSettings.LogStateMachine);
            stateMachine.AddReference("Card", this);
            stateMachine.AddReference("MovingContainer", movingContainer);
            stateMachine.AddReference("Board", board);
            stateMachine.AddReference("Player", player);
            
            stateMachine.SetState(new HeldState());
        }
    }
}

