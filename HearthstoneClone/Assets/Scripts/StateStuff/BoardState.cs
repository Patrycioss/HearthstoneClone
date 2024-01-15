using CardManagement.CardComposition;
using CardManagement.Physical;
using Game;

namespace StateStuff
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is put on the board.
	/// </summary>
	public class BoardState : State
	{
		private PhysicalCard card;
		private Board board;

		public override void Start()
		{
			StateMachine.GetReference("Card", out card);
			StateMachine.GetReference("Board", out board);
			
			board.TryAddCard(card);
			
			switch (card.CardInfo.Type)
			{
				case CardType.Minion:
					break;
				case CardType.Spell:
					//Todo: cast spell.
					break;
				case CardType.Weapon:
					//Todo: equip weapon.
					break;
			}
		}

		public override void Update()
		{
		}

		public override void Stop()
		{
		}
	}
}