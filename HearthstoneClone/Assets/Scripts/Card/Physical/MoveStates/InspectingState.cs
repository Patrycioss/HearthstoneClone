using System;
using UnityEngine;

namespace Card.Physical.MoveStates
{
	/// <summary>
	/// State that handles what happens when the player is inspecting a <see cref="PhysicalCard"/>.
	/// </summary>
	public class InspectingState : CustomStates.MovePhysicalCardState
	{
		private Transform canvasParent;
		private Vector3 startPosition;
		private Quaternion startRotation;

		private Camera activeCamera;

		// private TweenerCore<Vector3, Vector3, VectorOptions> inspectMoveTween;
		// private TweenerCore<Quaternion, Vector3, QuaternionOptions> inspectRotateTween;

		public InspectingState(PhysicalCard pPhysicalCard, Transform canvasParent, Vector3 startPosition, Quaternion startRotation) : base(pPhysicalCard)
		{
			this.canvasParent = canvasParent;

			this.startPosition = startPosition;
			this.startRotation = startRotation;

			activeCamera = Camera.main;
		}

		public override void Start()
		{
			float hWidth = Screen.width / 2f;
			float newX = -(hWidth - Input.mousePosition.x) / hWidth;
        
			Transform mainCamTransform = activeCamera.transform;
			Vector3 newPos = mainCamTransform.position + mainCamTransform.forward * 1.4f;
			newPos.x += newX;

			canvasParent.position = newPos;
			canvasParent.rotation = startRotation * Quaternion.Euler(-20, 0, 0);

			// inspectMoveTween = canvasParent.DOMove(newPos, 1).SetEase(Ease.OutCubic);
			// inspectRotateTween = canvasParent.DORotate((startRotation * Quaternion.Euler(-20,0,0)).eulerAngles, 1).SetEase(Ease.OutCubic);
		}

		public override void Update() {}

		public override void Stop(Action onCompleteCallback)
		{
			canvasParent.position = startPosition;
			canvasParent.rotation = startRotation;
			
			onCompleteCallback();
			
			// bool moveDone = false;
			// bool rotateDone = false;

			// inspectMoveTween = canvasParent.DOMove(startPosition, 1).SetEase(Ease.OutCubic).OnComplete(() =>
			// {
			// 	moveDone = true;
			// 	if (rotateDone) onCompleteCallback();
			// });
			//      
			// inspectRotateTween = canvasParent.DORotate(startRotation.eulerAngles, 1).SetEase(Ease.OutCubic).OnComplete(() =>
			// {
			// 	rotateDone = true;
			// 	if (moveDone) onCompleteCallback();
			// });
		}
	}
}