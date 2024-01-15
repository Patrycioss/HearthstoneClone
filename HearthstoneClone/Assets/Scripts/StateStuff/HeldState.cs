using CardManagement.Physical;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace StateStuff
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is in the player's hand.
	/// </summary>
	public class HeldState : State
	{
		private PhysicalCard card;
		private Player player;

		public override void Start()
		{
			StateMachine.GetReference("Card", out card);
			StateMachine.GetReference("Player", out player);

			card.transform.SetParent(player.Hand);
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) player.Hand);
		}

		public override void Update()
		{
			if (card.IsHoveringOver)
			{
				StateMachine.SetState(new InspectingState());
			}
		}

		public override void Stop()
		{
		}
	}
}