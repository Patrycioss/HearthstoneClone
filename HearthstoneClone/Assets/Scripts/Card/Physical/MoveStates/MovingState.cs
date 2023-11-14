using System;
using UnityEngine;
using State = CustomStates.State;

namespace Card.Physical.MoveStates
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is being moved by the player.
	/// </summary>
	public class MovingState : CustomStates.MovePhysicalCardState
	{
		private const float CLIP_PLANE_CONSTANT = 5.8f;
		
		private Camera camera;
		private Vector3 movingOffset;

		private Vector3 startPos;

		private State nextState;
		
		public MovingState(PhysicalCard card, State nextState) : base(card)
		{
			camera = Camera.main;

			startPos = card.transform.position;
			this.nextState = nextState;
		}

		public override void Start()
		{
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = camera.nearClipPlane + 5.7f;

			movingOffset = camera.ScreenToWorldPoint(mousePos) - physicalCard.transform.position;
		}

		public override void Update()
		{
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = CLIP_PLANE_CONSTANT;
			
			Vector3 newPosition = camera.ScreenToWorldPoint(mousePos) - movingOffset;
			physicalCard.transform.position = new Vector3(newPosition.x, physicalCard.transform.position.y, newPosition.z);
			
			if (!(Input.GetMouseButton(0) || Input.GetMouseButton(1)))
			{
			    stateMachine.SetState(nextState);
			}
		}

		public override void Stop(Action onCompleteCallback)
		{
			physicalCard.transform.position = startPos;
			onCompleteCallback();
			// card.transform.DOMove(startPos, Card.GetMoveDuration(card.transform.position, startPos))
			// 	.SetEase(Ease.OutCubic).OnComplete(() =>
			// {
			// 	onCompleteCallback();
			// });
		}
	}
}