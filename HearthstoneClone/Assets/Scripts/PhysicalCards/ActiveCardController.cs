using CardComposition;
using Game;

namespace PhysicalCards
{
	/// <summary>
	/// Controls the stats of an active card in the game.
	/// </summary>
	public class ActiveCardController
	{
		/// <summary>
		/// Amount of attack of the card.
		/// </summary>
		public int Attack { get; private set; }

		/// <summary>
		/// Cost of the card.
		/// </summary>
		public int Cost { get; private set; }

		private readonly ResourceContainer health;
		private readonly PhysicalCardVisuals visuals;
		
		public ActiveCardController(CardInfo cardInfo,PhysicalCardVisuals cardVisuals)
		{
			visuals = cardVisuals;

			SetAttack(cardInfo.Attack);
			
			health = new ResourceContainer(0);
			ChangeHealth(cardInfo.Health);
			
			SetCost(cardInfo.Cost);

			visuals.Attack.Show(true);

			if (cardInfo.Type != CardType.Spell)
			{
				visuals.Health.Show(true);
			}
		}

		/// <summary>
		/// Change the health.
		/// </summary>
		public void ChangeHealth(int amount)
		{
			health.AddOrRemove(amount);
			visuals.Health.SetText(health.Total.ToString());
		}
		
		/// <summary>
		/// Set the attack.
		/// </summary>
		/// <param name="amount"></param>
		public void SetAttack(int amount)
		{
			Attack = amount;
			visuals.Attack.SetText(Attack.ToString());
		}

		/// <summary>
		/// Set the cost.
		/// </summary>
		/// <param name="amount"></param>
		public void SetCost(int amount)
		{
			Cost = amount;
			visuals.Cost.SetText(Cost.ToString());
		}
	}
}