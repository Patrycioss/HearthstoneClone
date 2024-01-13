using CardManagement.Physical;

namespace FancyStateStuff
{
	public abstract class FancyMovePhysicalCardFancyState : FancyState
	{
		protected PhysicalCard PhysicalCard;
		
		public FancyMovePhysicalCardFancyState(PhysicalCard physicalCard)
		{
			PhysicalCard = physicalCard;
		}
	}
}