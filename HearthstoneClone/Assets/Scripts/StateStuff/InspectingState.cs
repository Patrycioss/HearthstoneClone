using CardManagement.Physical;
using UnityEngine;

namespace StateStuff
{
	/// <summary>
	/// State that handles what happens when the player is inspecting a <see cref="PhysicalCard"/>.
	/// </summary>
	public class InspectingState : MovePhysicalCardState
	{
		private const float SCALE_AMOUNT = 1.3f;
		
		public InspectingState(PhysicalCard pPhysicalCard) : base(pPhysicalCard)
		{
		}

		public override void Start()
		{
			PhysicalCard.transform.localScale = new Vector3(SCALE_AMOUNT, SCALE_AMOUNT, 1);
		}

		public override void Update()
		{
		}

		public override void Stop()
		{
			PhysicalCard.transform.localScale = Vector3.one;
		}
	}
}