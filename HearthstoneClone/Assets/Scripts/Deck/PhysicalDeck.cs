using UnityEngine;

namespace Deck
{
	/// <summary>
	/// Physical deck in the scene.
	/// </summary>
	public class PhysicalDeck : MonoBehaviour
	{
		/// <summary>
		/// <see cref="DeckInfo"/> that is currently active.
		/// </summary>
		public DeckInfo ActiveDeckInfo => activeDeckInfo;

		[SerializeField] private DeckInfo testDeckInfo;
		
		private DeckInfo activeDeckInfo;
		private PhysicalDeck instance;

		private void Awake()
		{
			if (instance != null)
			{
				Debug.LogWarning($"There's already another {typeof(PhysicalDeck)} in the scene!");
				Destroy(this);
			}
			instance = this;
			
			activeDeckInfo = testDeckInfo;
		}
	}
}