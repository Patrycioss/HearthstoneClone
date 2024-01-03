using System;
using System.Collections;
using CardManagement.CardComposition;
using CardManagement.Physical.MoveStates;
using HoverSystem;
using StateSystem;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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
    public class PhysicalCard : MonoBehaviour, IMouseOver
    {
        private const float CHECK_IF_HOLDING_TIME = 0.1f;
        
        /// <inheritdoc/>
        public bool IsHovering { get; set; }   
        
        [SerializeField] private Transform canvasParentTransform;
        [SerializeField] private Image image;
        
        private StateMachine stateMachine;
        private Vector3 startPos;
        private Quaternion startRot;

        private bool stoppedHolding;

        private Vector3 holdingOffset;

        private bool checkingIfConsciouslyHovering = false;
        private bool consciouslyHovering = false;

        private Location location = Location.None;

        private CardInfo cardInfo;

        /// <summary>
        /// Instantiate the physical card.
        /// </summary>
        /// <param name="initCardInfo">Info necessary to instantiate the physical card.</param>
        public void Instantiate(CardInfo initCardInfo)
        {
            cardInfo = initCardInfo;
            
            LoadSprite();
        }
        
        private void Awake()
        {
            stateMachine = new StateMachine();
        }

        private void Start()
        {
            startPos = transform.position;
            startRot = transform.rotation;
            
            stateMachine.SetState(new HeldState(this));
        }
        
        private void Update()
        {
            if (cardInfo == null)
            {
                return;
            }
            
            stateMachine.Update();

            cardInfo.Behaviours.ForEach(behaviour => behaviour.Update());

            if (IsHovering)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    OnClick();
                }
            }

            if (consciouslyHovering)
            {
                if (stateMachine.CurrentState is HeldState)
                {
                    stateMachine.SetState(new InspectingState(this, canvasParentTransform, startPos, startRot));
                }
            }
        }
        
        private void OnClick()
        {
            switch (location)
            {
                case Location.Board:
                    cardInfo.Behaviours.ForEach(behaviour => behaviour.OnInteract());
                    break;
                case Location.Hand:
                    StartCoroutine(CheckIfHolding());
                    break;
                case Location.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator CheckIfHolding()
        {
            yield return new WaitForSeconds(CHECK_IF_HOLDING_TIME);
            
            if (IsHovering && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
            {
                location = Location.None;
                cardInfo.Behaviours.ForEach(behaviour => behaviour.OnUse());

                stateMachine.SetState(new MovingState(this, new HeldState(this)));
            }
        }

        private IEnumerator CheckIfConsciouslyHovering()
        {
            checkingIfConsciouslyHovering = true;
            yield return new WaitForSeconds(0.1f);

            checkingIfConsciouslyHovering = false;
            
            if (IsHovering)
            {
                consciouslyHovering = true;
            }
        }

        public void OnStartHover()
        {
            if (!checkingIfConsciouslyHovering)
            {
                StartCoroutine(CheckIfConsciouslyHovering());
            }
            //
            // if (stateMachine.currentState is CardInHandState)
            // {
            //     Debug.Log("dit wel");
            //     stateMachine.SetState(new CardInspectingState(this, canvasParentTransform, startPos, startRot));
            // }
        }

        public void OnEndHover()
        {
            consciouslyHovering = false;
            
            if (stateMachine.CurrentState is InspectingState)
            {
                stateMachine.SetState(new HeldState(this));
            }
        }

        // public static float GetMoveDuration(Vector3 startPos, Vector3 targetPos)
        // {
        //     float duration = Vector3.Distance(startPos, targetPos) * CARD_MOVE_SPEED;
        //     return duration;
        // }

        private void LoadSprite()
        {
            if (cardInfo.ImageReference != null)
            {
                cardInfo.ImageReference.LoadAssetAsync<Sprite>().Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        image.sprite = handle.Result;
                    }
                    else
                    {
                        Debug.LogError($"Failed to load addressable asset: {cardInfo.ImageReference}!");
                    }
                };
            }
            else
            {
                Debug.LogError($"No image reference set for {cardInfo.CardName}!");
            }
        }
    }
}

