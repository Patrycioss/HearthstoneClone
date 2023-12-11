using Deck;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.DeckView
{
	/// <summary>
	/// Extension of <see cref="Footer"/> to handle the create deck button.
	/// </summary>
	public sealed class DeckFooter : Footer
	{
		[SerializeField] private ButtonContainer createDeckButton;

		protected override void OnEnable()
		{
			base.OnEnable();
			createDeckButton.AddListener(OnCreateDeckButtonClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			createDeckButton.RemoveListeners();
		}

		private void OnCreateDeckButtonClicked()
		{
			GameManager.Instance.AddTransferable("ActiveDeck",new DeckInfo());
			SceneManager.LoadSceneAsync("DeckEditor");
		}
	}
}