using System;
using UnityEngine;

namespace AssetManagement.Sprites
{
	/// <summary>
	/// Information necessary to store a sprite in the <see cref="SpriteManager"/>.
	/// </summary>
	[Serializable]
	public struct SpriteInformation
	{
		/// <summary>
		/// The name associated with the sprite.
		/// </summary>
		public string Name;
		
		/// <summary>
		/// The actual sprite.
		/// </summary>
		public Sprite Sprite;
	}
}