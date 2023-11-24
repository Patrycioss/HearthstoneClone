using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace AssetManagement.Sprites
{
	/// <summary>
	/// Singleton that manages the sprites that are used by the cards.
	/// </summary>
	public class SpriteManager : MonoBehaviour
	{
		/// <summary>
		/// Instance of the singleton.
		/// </summary>
		public static SpriteManager Instance;
		
		[SerializeField] private List<SpriteInformation> sprites;

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}


		/// <summary>
		/// Returns a sprite with the given name.
		/// </summary>
		/// <param name="spriteName">Name of the sprite.</param>
		/// <returns></returns>
		[CanBeNull]
		public Sprite GetSprite(string spriteName)
		{
			return sprites.Find(information => information.Name == spriteName).Sprite;
		}
	}
}