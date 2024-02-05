using PhysicalCards;
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
			if (Input.GetMouseButtonDown(0))
			{
				if (card.HasBeenPlayed)
				{
					if (!card.IsLocked)
					{							
						if (card.IsAwake)
						{
							card.Select();
						}
					}
				}
				else if (!card.IsLocked)
				{
					StateMachine.SetState(new MovingState());
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				if (card.HasBeenPlayed)
				{
					card.Deselect();
				}
			}
			
			if (!card.IsHoveringOver)
			{
				StateMachine.SetState(card.HasBeenPlayed? new BoardState() : new HeldState());
			}
		}

		public override void Stop()
		{
			card.transform.localScale = Vector3.one;
		}
	}
}