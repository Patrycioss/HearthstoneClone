using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardComposition;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Utils
{
	/// <summary>
	/// Utility functions to load addressables.
	/// </summary>
	public static class ResourceUtils
	{
		#region IResourceLocation
		
		/// <summary>
		/// Provider ID used by all the <see cref="IResourceLocation"/>.
		/// </summary>
		public static readonly string PROVIDER_ID = "UnityEngine.ResourceManagement.ResourceProviders.AssetDatabaseProvider";
		
		/// <summary>
		/// Construct an <see cref="IResourceLocation"/> from a <see cref="string"/> identifier.
		/// </summary>
		/// <param name="identifier"><see cref="string"/> identifier.</param>
		/// <returns>An <see cref="IResourceLocation"/>.</returns>
		public static IResourceLocation GetLocation(string identifier)
		{
			return new ResourceLocationBase(identifier, identifier, PROVIDER_ID,
				typeof(CardInfo), Array.Empty<IResourceLocation>());
		}

		/// <summary>
		/// Constructs a list of <see cref="IResourceLocation"/> from a list of <see cref="string"/> identifiers.
		/// </summary>
		/// <param name="identifiers"><see cref="IEnumerable{T}"/> of <see cref="string"/> identifier."/></param>
		/// <returns>A list of <see cref="IResourceLocation"/>.</returns>
		public static IList<IResourceLocation> GetLocations(IEnumerable<string> identifiers)
		{
			return identifiers.Select(GetLocation).ToList();
		}
		
		#endregion
		
		#region LoadAddressable
		
		/// <summary>
		/// Load an addressable from an <see cref="AssetReference"/>. 
		/// </summary>
		/// <param name="assetReference"><see cref="AssetReference"/> to load from.</param>
		/// <param name="onSucceedCallback">Callback called when loading is successful.</param>
		/// <typeparam name="T">Type of the addressable to load.</typeparam>
		/// <returns>The loaded addressable.</returns>
		public static async Task<T> LoadAddressableFromReference<T>(AssetReference assetReference, Action onSucceedCallback = default)
		{
			return await LoadAddressableFromHandle(Addressables.LoadAssetAsync<T>(assetReference), onSucceedCallback);
		}

		/// <summary>
		/// Loads an addressable from a <see cref="IResourceLocation"/>.
		/// </summary>
		/// <param name="resourceLocation"><see cref="IResourceLocation"/> to load from.</param>
		/// <param name="onSucceedCallback">Callback called when loading is successful.</param>
		/// <typeparam name="T">Type of the addressable.</typeparam>
		/// <returns>The loaded addressable.</returns>
		public static async Task<T> LoadAddressableFromLocation<T>(IResourceLocation resourceLocation, Action onSucceedCallback = default)
		{
			return await LoadAddressableFromHandle(Addressables.LoadAssetAsync<T>(resourceLocation), onSucceedCallback);
		}
		
		/// <summary>
		/// Loads an addressable from a <see cref="AssetLabelReference"/>.
		/// </summary>
		/// <param name="assetLabelReference"><see cref="AssetLabelReference"/> of the asset.</param>
		/// <param name="onSucceedCallback">Callback called when loading is successful.</param>
		/// <typeparam name="T">Type of the addressable.</typeparam>
		/// <returns>The loaded addressable.</returns>
		public static async Task<T> LoadAddressableFromLabel<T>(AssetLabelReference assetLabelReference, Action onSucceedCallback = default)
		{
			return await LoadAddressableFromHandle(Addressables.LoadAssetAsync<T>(assetLabelReference), onSucceedCallback);
		}

		/// <summary>
		/// Loads an addressable from a <see cref="string"/> identifier.
		/// </summary>
		/// <param name="identifier"><see cref="string"/> identifier.</param>
		/// <param name="onSucceedCallback">Callback called when loading is successful.</param>
		/// <typeparam name="T">Type of the addressable.</typeparam>
		/// <returns>The loaded addressable.</returns>
		public static async Task<T> LoadAddressableFromIdentifier<T>(string identifier, Action onSucceedCallback = default)
		{
			return await LoadAddressableFromLocation<T>(GetLocation(identifier), onSucceedCallback);
		}
		
		/// <summary>
		/// Loads an addressable from a <see cref="AsyncOperationHandle{T}"/>.
		/// </summary>
		/// <param name="handle"><see cref="AsyncOperationHandle{T}"/> used for loading.</param>
		/// <param name="onSucceedCallback">Callback called when loading is successful.</param>
		/// <typeparam name="T">Type of the addressable.</typeparam>
		/// <returns>The loaded addressable.</returns>
		public static async Task<T> LoadAddressableFromHandle<T>(AsyncOperationHandle<T> handle, Action onSucceedCallback = default)
		{
			await handle.Task;

			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				onSucceedCallback?.Invoke();
				return handle.Result;
			}

			Debug.LogError($"Failed to load asset of type {typeof(T)} {handle.OperationException}!");
			return default;
		}
		
		#endregion LoadAddressable

		#region LoadAddressables
		
		/// <summary>
		/// Loads addressables from a <see cref="AssetLabelReference"/>.
		/// </summary>
		/// <remarks>Useful for loading from a folder if it's labeled.</remarks>
		/// <param name="assetLabelReference"><see cref="AssetLabelReference"/> of the assets.</param>
		/// <param name="onSucceedCallback">Callback called when loading is successful.</param>
		/// <param name="callback">Callback called whenever a single addressable has loaded.</param>
		/// <typeparam name="T">Type of the addressable.</typeparam>
		/// <returns>The loaded addressables.</returns>
		public static async Task<IList<T>> LoadAddressables<T>(AssetLabelReference assetLabelReference, 
			Action onSucceedCallback = default, 
			Action<T> callback = default)
		{
			return await LoadAddressableFromHandle(Addressables.LoadAssetsAsync(assetLabelReference, callback), onSucceedCallback);
		}
		
		/// <summary>
		/// Loads addressables from a list of generic keys.
		/// </summary>
		/// <param name="keys">List of generic keys.</param>
		/// <param name="onSucceedCallback">Callback called when loading is successful.</param>
		/// <param name="callback">Callback called whenever a single addressable has loaded.</param>
		/// <typeparam name="T">Type of the addressable.</typeparam>
		/// <returns>The loaded addressables.</returns>
		public static async Task<IList<T>>LoadAddressablesFromKeys<T>(IEnumerable<object> keys, 
			Action onSucceedCallback = default, 
			Action<T> callback = default)
		{
			return await LoadAddressableFromHandle(Addressables.LoadAssetsAsync(keys, callback), onSucceedCallback);
		}
		
		/// <summary>
		/// Loads addressable from a list of <see cref="IResourceLocation"/>.
		/// </summary>
		/// <param name="resourceLocations">List of <see cref="IResourceLocation"/>.</param>
		/// <param name="onSucceedCallback">Callback called when loading is successful.</param>
		/// <param name="callback">Callback called whenever a single addressable has loaded.</param>
		/// <typeparam name="T">Type of the addressable.</typeparam>
		/// <returns>The loaded addressables.</returns>
		public static async Task<IList<T>> LoadAddressablesFromLocations<T>(IList<IResourceLocation> resourceLocations, 
			Action onSucceedCallback = default, 
			Action<T> callback = default)
		{
			return await LoadAddressableFromHandle(Addressables.LoadAssetsAsync(resourceLocations, callback), onSucceedCallback);
		}
		
		/// <summary>
		/// Loads addressables from a list of <see cref="string"/> identifiers.
		/// </summary>
		/// <param name="identifiers">List of <see cref="string"/> identifers.</param>
		/// <param name="onSucceedCallback">Callback called when loading is successful.</param>
		/// <param name="callback">Callback called whenever a single addressable has loaded.</param>
		/// <typeparam name="T">Type of the addressable.</typeparam>
		/// <returns>The loaded addressables.</returns>
		public static async Task<IList<T>> LoadAddressablesFromIdentifiers<T>(IEnumerable<string> identifiers,
			Action onSucceedCallback = default,
			Action<T> callback = default)
		{
			return await LoadAddressablesFromLocations(GetLocations(identifiers), onSucceedCallback, callback);
		}
		
		#endregion

		#region InstantiatePrefab

		/// <summary>
		/// Instantiates a prefab from a <see cref="AssetLabelReference"/>.
		/// </summary>
		/// <param name="assetLabelReference"><see cref="AssetLabelReference"/> of the prefab.</param>
		/// <param name="instantiationParameters">Parameters to instantiate the prefab with.</param>
		/// <param name="onSucceedCallback">Callback called when instantiating is successful.</param>
		/// <param name="trackHandle">If true, Addressables will track this request to allow it to be released via the result object.</param>
		/// <returns>The instantiated prefab.</returns>
		public static async Task<GameObject> InstantiateFromLabel(AssetLabelReference assetLabelReference, 
			InstantiationParameters instantiationParameters,
			Action onSucceedCallback = default,
			bool trackHandle = true)
		{
			return await InstantiateFromHandle(Addressables.InstantiateAsync(assetLabelReference, instantiationParameters, trackHandle), onSucceedCallback);
		}

		/// <summary>
		/// Instantiates an addressable from a <see cref="AsyncOperationHandle{T}"/>.
		/// </summary>
		/// <param name="handle"><see cref="AsyncOperationHandle{T}"/> used for loading.</param>
		/// <param name="onSucceedCallback">Callback called when instantiating is successful.</param>
		/// <returns>The instantiated prefab.</returns>
		public static async Task<GameObject> InstantiateFromHandle(AsyncOperationHandle<GameObject> handle, Action onSucceedCallback = default)
		{
			await handle.Task;

			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				onSucceedCallback?.Invoke();
				return handle.Result;
			}

			Debug.LogError($"Failed to instantiate prefab because: {handle.OperationException}!");
			return default;
		}

		#endregion
		
		#region InstantiatePrefabs

		

		#endregion
	}
}