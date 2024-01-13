using System.Threading;
using System.Threading.Tasks;
using CardManagement.Physical;

namespace FancyStateStuff
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is in the player's hand.
	/// </summary>
	public class FancyHeldState : FancyMovePhysicalCardFancyState
	{
		public FancyHeldState(PhysicalCard card) : base(card) {}

		public override Task Start(CancellationToken fastForwardToken)
		{
			return Task.CompletedTask;
		}

		public override Task Update()
		{
			return Task.CompletedTask;
		}

		public override Task Stop(CancellationToken fastForwardToken)
		{
			return Task.CompletedTask;
		}
	}
}