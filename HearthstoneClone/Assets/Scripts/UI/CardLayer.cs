using System;
using CardManagement.CardComposition;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
	public class CardLayer : MonoBehaviour
	{
		[SerializeField] private ButtonContainer buttonContainer;

		private Action callback;
		
		public void Initialize(CardInfo cardInfo, Action<CardInfo, GameObject> onPressedCallback)
		{
			buttonContainer.TextMesh.text = cardInfo.CardName;
			buttonContainer.Graphic.color = Random.ColorHSV();
			callback = () => onPressedCallback?.Invoke(cardInfo, gameObject);
		}

		private void OnClick()
		{
			callback?.Invoke();
		}
		
		private void OnEnable()
		{
			buttonContainer.AddListener(OnClick);
		}

		private void OnDisable()
		{
			buttonContainer.RemoveListeners();
		}
	}
}