using System.Threading;
using System.Threading.Tasks;
using CardManagement.Physical;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace FancyStateStuff
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is being moved by the player.
	/// </summary>
	public class FancyMovingState : FancyMovePhysicalCardFancyState
	{
		private const float SPEED_FACTOR = 700;

		public FancyMovingState(PhysicalCard card) : base(card) {}

		public override Task Start(CancellationToken fastForwardToken)
		{
			return Task.CompletedTask;
		}

		public override Task Update()
		{
			// PhysicalCard.transform.position = Input.mousePosition;
			return Task.CompletedTask;
		}
		
		public override Task Stop(CancellationToken fastForwardToken)
		{
			return Task.CompletedTask;
			// float duration = Vector3.Distance(PhysicalCard.BasePosition, PhysicalCard.transform.position) / SPEED_FACTOR;
			// var tween = PhysicalCard.transform.DOMove(PhysicalCard.BasePosition, duration);
			// await tween.AsFastForwardTask(fastForwardToken);
		}
	}
}