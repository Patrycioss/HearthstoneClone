using PhysicalCards;

namespace CardBehaviours
{
	/// <summary>
	/// Behaviour that handles attacking a target.
	/// </summary>
	public class AttackBehaviour : TargetBehaviour
	{
		protected override void OnTargetSelected(PhysicalCard card)
		{
			base.OnTargetSelected(card);
			
		}
	}
}