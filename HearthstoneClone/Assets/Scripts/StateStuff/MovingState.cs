using CardManagement.Physical;
using UnityEngine;

namespace StateStuff
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is being moved by the player.
	/// </summary>
	public class MovingState : MovePhysicalCardState
	{
		public MovingState(PhysicalCard card) : base(card) {}

		public override void Start()
		{
		}

		public override void Update()
		{
			PhysicalCard.transform.position = Input.mousePosition;
		}
		
		public override void Stop()
		{
			PhysicalCard.transform.position = PhysicalCard.BasePosition;
		}
	}
}