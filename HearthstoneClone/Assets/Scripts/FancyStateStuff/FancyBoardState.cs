using System.Threading;
using System.Threading.Tasks;
using CardManagement.CardComposition;
using CardManagement.Physical;

namespace FancyStateStuff
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is put on the board.
	/// </summary>
	public class FancyBoardState : FancyMovePhysicalCardFancyState
	{
		public FancyBoardState(PhysicalCard card) : base(card) {}

		public override Task Start(CancellationToken fastForwardToken)
		{
			switch (PhysicalCard.CardInfo.Type)
			{
				case CardType.Minion:
					
					break;
				case CardType.Spell:
					//Todo: cast spell.
					break;
				case CardType.Weapon:
					//Todo: equip weapon.
					break;
			}
			
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