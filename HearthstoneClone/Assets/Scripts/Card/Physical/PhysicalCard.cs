using System.Collections;
using AssetManagement.Sprites;
using Card.Physical.MoveStates;
using HoverSystem;
using StateSystem;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


namespace Card.Physical
{
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

        private CardInfo cardInfo;

        /// <summary>
        /// Instantiate the physical card.
        /// </summary>
        /// <param name="initCardInfo">Info necessary to instantiate the physical card.</param>
        public void Instantiate(CardInfo initCardInfo)
        {
            cardInfo = initCardInfo;
            image.sprite = SpriteManager.Instance.GetSprite(initCardInfo.SpriteName);
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
            stateMachine.Update();

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
            StartCoroutine(CheckIfHolding());
        }

        private IEnumerator CheckIfHolding()
        {
            yield return new WaitForSeconds(CHECK_IF_HOLDING_TIME);
            
            if (IsHovering && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
            {
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
    }
}

