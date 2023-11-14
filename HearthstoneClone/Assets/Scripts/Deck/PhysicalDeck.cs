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

		[Header("TESTING")]
		[SerializeField] private DeckInfo testDeckInfo;
		[SerializeField] private bool testing = false;
		
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

			if (testing)
			{
				activeDeckInfo = testDeckInfo;
			}
		}
	}
}