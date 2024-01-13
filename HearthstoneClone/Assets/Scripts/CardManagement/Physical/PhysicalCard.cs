using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardManagement.CardComposition;
using CardManagement.CardComposition.Behaviours;
using CardManagement.Physical.MoveStates;
using JetBrains.Annotations;
using StateSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace CardManagement.Physical
{
    public enum Location
    {
        Board,
        Hand,
        None,
    }
    
    /// <summary>
    /// Physical card in the game.
    /// </summary>
    public class PhysicalCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const int CONSCIOUS_HOVERING_TIME_MILLIS = 50;
        
        private enum Side
        {
            Front,
            Back,
        }

        /// <summary>
        /// Can the card be moved?
        /// </summary>
        public bool IsLocked { get; set; }
        
        /// <summary>
        /// Base position of the card.
        /// </summary>
        public Vector3 BasePosition { get; set; }

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Image image;

        [SerializeField] private GameObject frontObject;
        [SerializeField] private GameObject backObject;
        
        private StateMachine stateMachine;
        private CardInfo cardInfo;

        private Side cardSide = Side.Back;
        private bool consciouslyHovering = false;
        private bool hovering = false;
        private bool moving = false;

        [CanBeNull] private Task checkConsciousHoverTask;
        [CanBeNull] private Task setStateToInspectingTask;
        private CancellationTokenSource cancelConsciousTimerToken = new CancellationTokenSource();
        private bool initialized = false;

        private Queue<Action> doNextUpdate = new Queue<Action>();
        
        /// <summary>
        /// Instantiate the physical card.
        /// </summary>
        /// <param name="initCardInfo">Info necessary to instantiate the physical card.</param>
        public async void Instantiate(CardInfo initCardInfo)
        {
            stateMachine = new StateMachine(new HeldState(this));
            
            cardInfo = initCardInfo;

            title.text = initCardInfo.CardName;
            description.text = initCardInfo.Description;
            
            image.sprite = await LoadSprite();
            initialized = true;

            BasePosition = transform.position;
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
        
        private void Update()
        {
            while (doNextUpdate.Count > 0)
            {
                doNextUpdate.Dequeue()?.Invoke();
            }
            
            if (!initialized)
            {
                return;
            }

            if (hovering)
            {
                if (Input.GetMouseButton(0) && !moving)
                {
                    moving = true;
                    stateMachine.FastForwardNextStateStartTask();
                    stateMachine.FastForwardNextStateStopTask();
                    stateMachine.SetState(new MovingState(this));
                }
                else if (!Input.GetMouseButton(0) && moving)
                {
                    moving = false;
                    stateMachine.SetState(new HeldState(this));
                }
            }

            stateMachine?.Update();

            foreach (CardBehaviour behaviour in cardInfo.Behaviours)
            {
                behaviour.Update();
            }
        }
        
        private async Task<Sprite> LoadSprite()
        {
            return await ResourceUtils.LoadAddressableFromReference<Sprite>(cardInfo.ImageReference);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovering = true;
            
            if (stateMachine.ActiveState is HeldState)
            {
                stateMachine.SetState(new InspectingState(this));
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
          
            if (stateMachine.ActiveState is not null or HeldState)
            {
                stateMachine.SetState(new HeldState(this));
            }
            
            consciouslyHovering = false;
        }
    }
}

