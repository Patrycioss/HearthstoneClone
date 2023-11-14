using Card.Physical;

namespace StateSystem
{
	public abstract class MovePhysicalCardState : State
	{
		protected PhysicalCard physicalCard;
		
		public MovePhysicalCardState(PhysicalCard physicalCard)
		{
			this.physicalCard = physicalCard;
		}
	}
}