using System.Threading;
using System.Threading.Tasks;
using CardManagement.Physical;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace FancyStateStuff
{
	/// <summary>
	/// State that handles what happens when the player is inspecting a <see cref="PhysicalCard"/>.
	/// </summary>
	public class FancyInspectingState : FancyMovePhysicalCardFancyState
	{
		private const float SCALE_AMOUNT = 1.3f;
		private const float SCALE_DURATION = 0.5f;
		
		public FancyInspectingState(PhysicalCard pPhysicalCard) : base(pPhysicalCard)
		{
		}

		public override async Task Start(CancellationToken fastForwardToken)
		{
			var tween =PhysicalCard.transform.DOScale(new Vector3(SCALE_AMOUNT, SCALE_AMOUNT, 1), SCALE_DURATION)
				.SetEase(Ease.OutCubic);
			
			await tween.AsFastForwardTask(fastForwardToken);
		}

		public override Task Update()
		{
			return Task.CompletedTask;
		}

		public override async Task Stop(CancellationToken fastForwardToken)
		{
			var tween = 
				PhysicalCard.transform.DOScale(Vector3.one, SCALE_DURATION).SetEase(Ease.OutCubic);
				
			await tween.AsFastForwardTask(fastForwardToken);
		}
	}
}