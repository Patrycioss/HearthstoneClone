using CardManagement.Physical;
using UnityEngine;

namespace CardBehaviours
{
	public class TargetBehaviour : CardBehaviour
	{
		[SerializeField] private GameObject imageRoot;

		private GameObject selector;
		private Vector3 startPos;

		public override void OnPlay()
		{
			base.OnPlay();
			startPos = transform.position;
		}

		public override void Update()
		{
			if (Selected)
			{
				transform.position = Input.mousePosition;
			}
		}
		
		public override void OnSelect()
		{
			base.OnSelect();

			imageRoot.SetActive(true);
		}

		public override void OnDeselect()
		{
			base.OnDeselect();
			transform.position = startPos;
			imageRoot.SetActive(false);

			//Todo: check what card it's hovering over
			Debug.DrawRay(transform.position, Vector3.forward);
			if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit))
			{ 
				Debug.LogError($"{hit.transform.name}");

				if (hit.transform.TryGetComponent(out PhysicalCard card))
				{
					OnTargetSelected(card);
				}
			}
		}

		protected virtual void OnTargetSelected(PhysicalCard card)
		{
			Debug.Log($"Selected card with name: {card.CardInfo.CardName}");
		}
	}
}