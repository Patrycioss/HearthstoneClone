using PhysicalCards;
using UnityEngine;

namespace CardBehaviours
{
	public class FredBehaviour : TargetBehaviour
	{
		protected override void OnTargetSelected(PhysicalCard card)
		{
			base.OnTargetSelected(card);
			
			Debug.Log($"Selected card: {card.CardInfo.CardName}");
		}
	}
}