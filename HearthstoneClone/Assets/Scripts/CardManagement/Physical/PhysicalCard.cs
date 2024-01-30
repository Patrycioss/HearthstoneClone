using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CardManagement.CardComposition;
using CardManagement.CardComposition.Behaviours;
using Extensions;
using Game;
using Settings;
using StateStuff;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace CardManagement.Physical
{
    /// <summary>
    /// Physical card in the game.
    /// </summary>
    public class PhysicalCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const int ERROR_REMOVE_DELAY_MS = 2000;
        
        /// <summary>
        /// Error the card can show when tried to be played.
        /// </summary>
        public enum Error
        {
            NotEnoughMana,
            BoardIsFull,
            None
        }
        
        /// <summary>
        /// Side of the card.
        /// </summary>
        private enum Side
        {
            Front,
            Back,
        }

        /// <summary>
        /// Can the card not be moved?
        /// </summary>
        public bool IsLocked
        {
            get => locked;
            set
            {
                Debug.Log($"Has locked: {CardInfo.name}? {value}");
                locked = value;
            }
        }

        /// <summary>
        /// Determines whether the card can be used on the board, only applies to minions.
        /// </summary>
        public bool IsAwake { get; set; } = false;

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

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image image;
        [SerializeField] private IconController costRoot;
        [SerializeField] private IconController healthIcon;
        [SerializeField] private IconController attackIcon;
        [SerializeField] private GameObject frontObject;
        [SerializeField] private GameObject backObject;
        [SerializeField] private GameObject notEnoughManaError;
        [SerializeField] private GameObject boardIsFullError;
        [SerializeField] private LogSettings logSettings;

        private StateMachine stateMachine;
        private Transform playerHandTransform;
        private Transform movingContainer;
        private Board board;
        private Task setStateToInspectingTask;
        
        private Side cardSide = Side.Back;
        private bool initialized = false;
        private bool locked = false;

        private Error currentActiveError = Error.None;
        private CancellationTokenSource currentCancelSource = new CancellationTokenSource();
        private List<CardBehaviour> behaviours = new List<CardBehaviour>();
        
        /// <summary>
        /// Initialize the physical card.
        /// </summary>
        /// <param name="configuration">Configuration for the physical card.</param>
        public async void Initialize(PhysicalCardConfiguration configuration)
        {
            CardInfo = configuration.CardInfo;
            title.text = configuration.CardInfo.CardName;
            description.text = configuration.CardInfo.Description;
            movingContainer = configuration.IntermediateContainer;
            board = configuration.Board;

            foreach (AssetReference reference in CardInfo.CardBehaviourReferences)
            {
                GameObject prefab = await ResourceUtils.LoadAddressableFromReference<GameObject>(reference);
                GameObject spawned = Instantiate(prefab, transform);
                
                if (spawned.TryGetComponent(out CardBehaviour behaviour))
                {
                    behaviours.Add(behaviour);
                }
            }
            
            stateMachine = new StateMachine(logSettings.LogStateMachine);
            stateMachine.AddReference("Card", this);
            stateMachine.AddReference("MovingContainer", movingContainer);
            stateMachine.AddReference("Board", board);
            stateMachine.AddReference("Player", configuration.Player);
            
            stateMachine.SetState(new HeldState());

            switch (CardInfo.Type)
            {
                case CardType.Minion or CardType.Weapon:
                    SetAttack(CardInfo.Attack);
                    ShowAttack(true);
                    
                    SetHealth(CardInfo.Health);
                    ShowHealth(true);
                    break;
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

            SetCost(CardInfo.Cost);
            ShowCost(true);
            
            
            image.sprite = await LoadSprite();
            initialized = true;
        }

        /// <summary>
        /// Show an error on the card that disappears after a few seconds.
        /// </summary>
        /// <param name="targetError">Error relating to playing the card.</param>
        public void ShowError(Error targetError)
        {
            if (currentActiveError != Error.None)
            {
                currentCancelSource.Cancel();
                SetErrorActive(currentActiveError, false);
            }

            currentActiveError = targetError;
            currentCancelSource = new CancellationTokenSource();
            SetErrorActive(targetError, true);

            _ = WaitToSetErrorInactive(targetError);
        }
        
        /// <summary>
        /// Set whether to show the mana cost of the card.
        /// </summary>
        public void ShowCost(bool show)
        {
            costRoot.gameObject.SetActive(show);
        }

        /// <summary>
        /// Set the cost of the card.
        /// </summary>
        /// <param name="amount">Amount to cost.</param>
        public void SetCost(int amount)
        {
            CardInfo.Cost = amount;
            costRoot.SetText(amount.ToString());
        }
        
        /// <summary>
        /// Set whether to show the health of the card.
        /// </summary>
        public void ShowHealth(bool show)
        {
            healthIcon.gameObject.SetActive(show);
        }

        /// <summary>
        /// Set the health of the card, useful for minions and weapons.
        /// </summary>
        /// <param name="amount">Amount of health.</param>
        public void SetHealth(int amount)
        {
            CardInfo.Health = amount;
            healthIcon.SetText(amount.ToString());
        }
        
        /// <summary>
        /// Set whether to show the attack of the card.
        /// </summary>
        public void ShowAttack(bool show)
        {
            attackIcon.gameObject.SetActive(show);
        }

        /// <summary>
        /// Set the attack of the card, useful for minions.
        /// </summary>
        /// <param name="amount">Amount of attack.</param>
        public void SetAttack(int amount)
        {
            CardInfo.Attack = amount;
            attackIcon.SetText(amount.ToString());
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
        /// Flip the card.
        /// </summary>
        public void Flip()
        {
            switch (cardSide)
            {
                case Side.Back:
                    cardSide = Side.Front;
                    frontObject.SetActive(true);
                    backObject.SetActive(false);
                    break;
                case Side.Front:
                    cardSide = Side.Back;
                    frontObject.SetActive(false);
                    backObject.SetActive(true);
                    break;
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
        
        private void SetErrorActive(Error targetError, bool setActive)
        {
            switch (targetError)
            {
                case Error.NotEnoughMana or Error.None:
                    notEnoughManaError.SetActive(setActive);
                    break;
                case Error.BoardIsFull or Error.None:
                    boardIsFullError.SetActive(setActive);
                    break;
            }
        }
        
        private async Task WaitToSetErrorInactive(Error error)
        {
            //Wait 5 seconds.
            await Task.Delay(ERROR_REMOVE_DELAY_MS, currentCancelSource.Token);
            SetErrorActive(error, false);
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
        
        private async Task<Sprite> LoadSprite()
        {
            return await ResourceUtils.LoadAddressableFromReference<Sprite>(CardInfo.ImageReference);
        }
    }
}

