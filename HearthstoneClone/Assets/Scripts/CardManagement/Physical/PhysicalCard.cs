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
            set => locked = value;
        }

        /// <summary>
        /// Base position of the card.
        /// </summary>
        public Vector3 BasePosition { get; set; }
        
        /// <summary>
        /// <see cref="CardInfo"/> associated with this physical card.
        /// </summary>
        public CardInfo CardInfo { get; set; }

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image image;
        [SerializeField] private GameObject costRoot;
        [SerializeField] private GameObject healthRoot;
        [SerializeField] private GameObject attackRoot;
        [SerializeField] private GameObject frontObject;
        [SerializeField] private GameObject backObject;

        private bool initialized = false;
        private bool hovering = false;
        private bool moving = false;
        private bool locked = false;
        private bool onBoard = false;

        private StateMachine stateMachine;
        private Side cardSide = Side.Back;
        private Player.TryPlayCallback onTryPlay;
        
        [CanBeNull] 
        private Task setStateToInspectingTask;


        /// <summary>
        /// Initialize the physical card.
        /// </summary>
        /// <param name="initCardInfo">Info necessary to initialize the physical card.</param>
        /// <param name="onTryPlayCallback">Called when the card is tried to play.</param>
        public async void Initialize(CardInfo initCardInfo, Player.TryPlayCallback onTryPlayCallback)
        {
            stateMachine = new StateMachine(new HeldState(this));
            
            CardInfo = initCardInfo;

            title.text = initCardInfo.CardName;
            description.text = initCardInfo.Description;
            
            image.sprite = await LoadSprite();
            initialized = true;

            BasePosition = transform.position;
            onTryPlay = onTryPlayCallback;
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
            hovering = true;
            
            if (stateMachine.ActiveState is not null or HeldState)
            {
                stateMachine.SetState(new InspectingState(this));
            }
        }

        /// <summary>
        /// Called when the mouse exits the card.
        /// </summary>
        /// <param name="eventData">Data associated with the event.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
          
            if (stateMachine.ActiveState is not null or HeldState)
            {
                stateMachine.SetState(new HeldState(this));
            }
        }
        
        private void Update()
        {
            if (!initialized)
            {
                return;
            }

            if (!onBoard && hovering)
            {
                if (Input.GetMouseButton(0) && !moving)
                {
                    moving = true;
                    stateMachine.SetState(new MovingState(this));
                }
                else if (!Input.GetMouseButton(0) && moving)
                {
                    if (onTryPlay(this))
                    {
                        stateMachine.SetState(new BoardState(this));
                        onBoard = true;
                    }
                    moving = false;
                    stateMachine.SetState(new HeldState(this));
                }
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

