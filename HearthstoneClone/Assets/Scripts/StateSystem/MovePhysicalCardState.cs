using CardManagement.Physical;

namespace StateSystem
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