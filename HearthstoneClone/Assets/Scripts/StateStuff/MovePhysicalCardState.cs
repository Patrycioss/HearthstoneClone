using CardManagement.Physical;

namespace StateStuff
{
	public abstract class MovePhysicalCardState : State
	{
		protected PhysicalCard PhysicalCard;
		
		public MovePhysicalCardState(PhysicalCard physicalCard)
		{
			PhysicalCard = physicalCard;
		}
	}
}