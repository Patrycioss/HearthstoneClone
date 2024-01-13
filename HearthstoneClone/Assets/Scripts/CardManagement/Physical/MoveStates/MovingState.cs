using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Extensions;
using StateSystem;
using UnityEngine;
using State = StateSystem.State;

namespace CardManagement.Physical.MoveStates
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is being moved by the player.
	/// </summary>
	public class MovingState : MovePhysicalCardState
	{
		private const float SPEED_FACTOR = 700;
		
		private Vector3 startPos;

		public MovingState(PhysicalCard card) : base(card)
		{
			startPos = card.transform.position;
		}

		public override Task Start(CancellationToken fastForwardToken)
		{
			return Task.CompletedTask;
		}

		public override Task Update()
		{
			PhysicalCard.transform.position = Input.mousePosition;
			return Task.CompletedTask;
		}
		
		public override async Task Stop(CancellationToken fastForwardToken)
		{
			float duration = Vector3.Distance(startPos, PhysicalCard.transform.position) / SPEED_FACTOR;
			var tween = PhysicalCard.transform.DOMove(startPos, duration);
			await tween.AsFastForwardTask(fastForwardToken);
		}
	}
}