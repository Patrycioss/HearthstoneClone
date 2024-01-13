using CardManagement.CardComposition;
using CardManagement.Physical;

namespace StateStuff
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is put on the board.
	/// </summary>
	public class BoardState : MovePhysicalCardState
	{
		public BoardState(PhysicalCard card) : base(card) {}

		public override void Start()
		{
			switch (PhysicalCard.CardInfo.Type)
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