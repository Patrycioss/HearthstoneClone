using System.Threading.Tasks;
using CardManagement.CardComposition;
using CardManagement.CardComposition.Behaviours;
using Game;
using JetBrains.Annotations;
using StateStuff;
using TMPro;
using UnityEngine;
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
        /// <see cref="CardInfo"/> associated with this physical card.
        /// </summary>
        public CardInfo CardInfo { get; private set; }
        
        /// <summary>
        /// Is the mouse hovering over the cared?
        /// </summary>
        public bool IsHoveringOver { get; private set; } = false;

        /// <summary>
        /// Is the card on the board right now?
        /// </summary>
        public bool IsOnBoard { get; set; } = false;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image image;
        [SerializeField] private GameObject costRoot;
        [SerializeField] private GameObject healthRoot;
        [SerializeField] private GameObject attackRoot;
        [SerializeField] private GameObject frontObject;
        [SerializeField] private GameObject backObject;

        private bool initialized = false;
        private bool locked = false;
        private StateMachine stateMachine;
        private Side cardSide = Side.Back;
        
        private Transform playerHandTransform;
        private Transform movingContainer;
        private Board board;
        
        [CanBeNull] 
        private Task setStateToInspectingTask;
        
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
            
            stateMachine = new StateMachine();
            stateMachine.AddReference("Card", this);
            stateMachine.AddReference("MovingContainer", movingContainer);
            stateMachine.AddReference("Board", board);
            stateMachine.AddReference("Player", configuration.Player);
            
            stateMachine.SetState(new HeldState());
            
            image.sprite = await LoadSprite();
            initialized = true;
        }

        /// <summary>
        /// Set whether to show the mana cost of the card.
        /// </summary>
        public void ShowCost(bool show)
        {
            costRoot.SetActive(show);
        }
        
        /// <summary>
        /// Set whether to show the health of the card.
        /// </summary>
        public void ShowHealth(bool show)
        {
            healthRoot.SetActive(show);
        }
        
        /// <summary>
        /// Set whether to show the attack of the card.
        /// </summary>
        public void ShowAttack(bool show)
        {
            attackRoot.SetActive(show);
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
        /// Called when the mouse exits the card.
        /// </summary>
        /// <param name="eventData">Data associated with the event.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            IsHoveringOver = false;
        }

        private void Update()
        {
            if (!initialized)
            {
                return;
            }
            
            stateMachine?.Update();

            foreach (CardBehaviour behaviour in CardInfo.Behaviours)
            {
                behaviour.Update();
            }
        }
        
        private async Task<Sprite> LoadSprite()
        {
            return await ResourceUtils.LoadAddressableFromReference<Sprite>(CardInfo.ImageReference);
        }
    }
}

