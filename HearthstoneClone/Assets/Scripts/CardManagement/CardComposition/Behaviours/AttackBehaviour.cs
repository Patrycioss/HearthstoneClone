using CardManagement.Physical;

namespace CardManagement.CardComposition.Behaviours
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