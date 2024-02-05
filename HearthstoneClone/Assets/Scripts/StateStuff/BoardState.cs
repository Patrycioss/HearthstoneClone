using CardComposition;
using Game;
using PhysicalCards;
using UnityEngine;

namespace StateStuff
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is put on the board.
	/// </summary>
	public class BoardState : State
	{
		private PhysicalCard card;
		private Transform movingContainer;

		public override void Start()
		{
			StateMachine.GetReference("Card", out card);
			StateMachine.GetReference("MovingContainer", out movingContainer);
			
			switch (card.CardInfo.Type)
			{
				case CardType.Minion:
					PlayManager.Instance.Board.AddCard(card);
					break;
				case CardType.Spell or CardType.Weapon:
					Transform transform = card.transform;
					transform.SetParent(movingContainer);
					transform.position = new Vector3(9999,999999,1);
					break;
			}
		}

		public override void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				card.Deselect();
			}
		}

		public override void Stop()
		{
		}
	}
}