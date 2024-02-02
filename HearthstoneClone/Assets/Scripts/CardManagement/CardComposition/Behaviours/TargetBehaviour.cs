using CardManagement.Physical;
using UnityEngine;

namespace CardManagement.CardComposition.Behaviours
{
	public class TargetBehaviour : CardBehaviour
	{
		[SerializeField] private GameObject selectorPrefab;

		private GameObject selector;

		public void SetTarget(PhysicalCard card)
		{
			OnTargetSelected(card);
			CleanUp();
		}

		public override void Update()
		{
			if (selector != null)
			{
				selector.transform.position = Input.mousePosition;
			}
		}
		
		public override void OnSelect()
		{
			base.OnSelect();

			selector = Instantiate(selectorPrefab, transform);
			if (selector != null)
			{
				selector.transform.localScale = Vector3.one;
			}
		}

		public override void OnDeselect()
		{
			base.OnDeselect();

			//Todo: check what card it's hovering over
			
			CleanUp();			
		}

		protected virtual void OnTargetSelected(PhysicalCard card)
		{
			Debug.Log($"Selected card with name: {card.CardInfo.CardName}");
		}

		protected virtual void CleanUp()
		{
			if (selector != null)
			{
				Destroy(selector);
			}
		}
	}
}