using Deck;
using UnityEngine;

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

		protected override void Back()
		{
			GameManager.Instance.SceneSwapper.SetScene(SceneSwapper.Scene.MainMenu);
		}

		private void OnCreateDeckButtonClicked()
		{
			GameManager.Instance.AddTransferable("ActiveDeck",new DeckInfo());
			GameManager.Instance.SceneSwapper.SetScene(SceneSwapper.Scene.DeckEditor);			
		}
	}
}