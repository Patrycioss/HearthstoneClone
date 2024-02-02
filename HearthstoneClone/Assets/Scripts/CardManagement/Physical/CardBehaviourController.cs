using System.Collections.Generic;
using CardManagement.CardComposition.Behaviours;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace CardManagement.Physical
{
	/// <summary>
	/// Manages the <see cref="CardBehaviour"/> on a <see cref="PhysicalCard"/>.
	/// </summary>
	public class CardBehaviourController
	{
		private List<CardBehaviour> behaviours = new List<CardBehaviour>();
		private bool instantiatedBehaviours = false;
		
		public CardBehaviourController(CardBehaviour behaviour, Transform behaviourParent)
		{
			InitializeBehaviours(behaviour, behaviourParent);
		}
		
		/// <summary>
		/// Call update on the active behaviours.
		/// </summary>
		public void CallUpdate()
		{
			if (!instantiatedBehaviours)
			{
				return;
			}
			
			foreach (CardBehaviour behaviour in behaviours)
			{
				behaviour.Update();
			}
		}

		/// <summary>
		/// Call OnPlay on the active behaviours.
		/// </summary>
		public void CallOnPlay()
		{
			if (!instantiatedBehaviours)
			{
				return;
			}
			
			behaviours.ForEach(behaviour => behaviour.OnPlay());
		}

		/// <summary>
		/// Call OnSelect on the active behaviours.
		/// </summary>
		public void CallOnSelect()
		{
			if (!instantiatedBehaviours)
			{
				return;
			}
			
			behaviours.ForEach(behaviour => behaviour.OnSelect());
		}

		/// <summary>
		/// Call OnDeselect on the active behaviours.
		/// </summary>
		public void CallOnDeselect()
		{
			if (!instantiatedBehaviours)
			{
				return;
			}
			
			behaviours.ForEach(behaviour => behaviour.OnDeselect());
		}
		
		private async void InitializeBehaviours(CardBehaviour behaviourReferences, Transform behaviourParent)
		{
			// foreach (AssetReference reference in behaviourReferences)
			// {
			// 	GameObject spawned = await ResourceUtils.InstantiateFromHandle(reference.InstantiateAsync(behaviourParent));
   //              
			// 	if (spawned.TryGetComponent(out CardBehaviour behaviour))
			// 	{
			// 		behaviours.Add(behaviour);
			// 	}
			// }

			// instantiatedBehaviours = true;
		}
	}
}