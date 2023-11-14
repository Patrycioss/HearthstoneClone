
using Card;
using Card.Physical;

namespace CustomStates
{
	public abstract class MovePhysicalCardState : State
	{
		protected PhysicalCard physicalCard;
		
		public MovePhysicalCardState(PhysicalCard pPhysicalCard)
		{
			this.physicalCard = pPhysicalCard;
		}
	}
}