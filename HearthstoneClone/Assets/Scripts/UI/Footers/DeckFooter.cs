using UnityEngine;
using UnityEngine.UI;

namespace UI.Footers
{
	/// <summary>
	/// Extension of <see cref="Footer"/> to handle the create deck button.
	/// </summary>
	public sealed class DeckFooter : Footer
	{
		[SerializeField] private Button createDeckButton;

		protected override void Awake()
		{
			base.Awake();
			createDeckButton.onClick.AddListener(OnCreateDeckButtonClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			createDeckButton.onClick.RemoveAllListeners();
		}

		private void OnCreateDeckButtonClicked()
		{
			//TODO: Go to deck editor with empty deck.
		}
	}
}