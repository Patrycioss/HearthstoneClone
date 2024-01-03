using System;
using System.Collections.Generic;
using CardManagement.CardComposition;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;
using Utils;

namespace UI
{
	public class UICard : MonoBehaviour
	{
		[SerializeField] private Button button;
		[SerializeField] private Image image;

		private Action callback;
		private List<AsyncOperationHandle> spriteHandles = new List<AsyncOperationHandle>(); 

		public void Initialize(CardInfo info, IResourceLocation resourceLocation, Action<CardInfo, IResourceLocation> pressCallback)
		{
			LoadSprite(info.ImageReference);

			callback = () => pressCallback?.Invoke(info, resourceLocation);
		}

		private void OnPressed()
		{
			callback?.Invoke();
		}

		private void OnEnable()
		{
			button.onClick.AddListener(OnPressed);
		}

		private void OnDisable()
		{
			button.onClick.RemoveAllListeners();
		}

		private void OnDestroy()
		{
			foreach (AsyncOperationHandle handle in spriteHandles)
			{
				Addressables.Release(handle);
			}
		}

		private async void LoadSprite(AssetReference imageReference)
		{
			Sprite sprite = await ResourceUtils.LoadAddressableFromReference<Sprite>(imageReference);
			image.sprite = sprite;
		}
	}
}