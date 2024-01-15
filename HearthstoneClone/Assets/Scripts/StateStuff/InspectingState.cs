using CardManagement.Physical;
using UnityEngine;

namespace StateStuff
{
	/// <summary>
	/// State that handles what happens when the player is inspecting a <see cref="PhysicalCard"/>.
	/// </summary>
	public class InspectingState : State
	{
		private const float SCALE_AMOUNT = 1.3f;
		private PhysicalCard card;
		
		public override void Start()
		{
			StateMachine.GetReference("Card", out card);
			card.transform.localScale = new Vector3(SCALE_AMOUNT, SCALE_AMOUNT, 1);
		}

		public override void Update()
		{
			if (card.IsHoveringOver)
			{
				if (Input.GetMouseButtonDown(0) && !card.IsOnBoard && !card.IsLocked)
				{
					StateMachine.SetState(new MovingState());
				}
			}
			
			if (!card.IsHoveringOver)
			{
				StateMachine.SetState(card.IsOnBoard? new BoardState() : new HeldState());
			}
		}

		public override void Stop()
		{
			card.transform.localScale = Vector3.one;
		}
	}
}