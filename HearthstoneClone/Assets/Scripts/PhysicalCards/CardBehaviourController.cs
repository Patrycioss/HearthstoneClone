using CardBehaviours;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace PhysicalCards
{
	/// <summary>
	/// Manages the <see cref="CardBehaviour"/> on a <see cref="PhysicalCard"/>.
	/// </summary>
	public class CardBehaviourController
	{
		private bool instantiatedBehaviour = false;
		private CardBehaviour behaviour;
		
		public CardBehaviourController(AssetReference behaviour, Transform behaviourParent)
		{
			InitializeBehaviour(behaviour, behaviourParent);
		}
		
		/// <summary>
		/// Call update on the active behaviours.
		/// </summary>
		public void CallUpdate()
		{
			if (!instantiatedBehaviour)
			{
				return;
			}
			
			behaviour.Update();
		}

		/// <summary>
		/// Call OnPlay on the active behaviours.
		/// </summary>
		public void CallOnPlay()
		{
			if (!instantiatedBehaviour)
			{
				return;
			}
			
			behaviour.OnPlay();
		}

		/// <summary>
		/// Call OnSelect on the active behaviours.
		/// </summary>
		public void CallOnSelect()
		{
			if (!instantiatedBehaviour)
			{
				return;
			}
			
			behaviour.OnSelect();
		}

		/// <summary>
		/// Call OnDeselect on the active behaviours.
		/// </summary>
		public void CallOnDeselect()
		{
			if (!instantiatedBehaviour)
			{
				return;
			}
			
			behaviour.OnDeselect();
		}
		
		private async void InitializeBehaviour(AssetReference behaviourReference, Transform behaviourParent)
		{
			GameObject spawned = await ResourceUtils.InstantiateFromHandle(behaviourReference.InstantiateAsync(behaviourParent));

			if (spawned.TryGetComponent(out behaviour))
			{
				instantiatedBehaviour = true;
			}
			else
			{
				Debug.LogError($"Couldn't find card behaviour on prefab!");
			}
		}
	}
}