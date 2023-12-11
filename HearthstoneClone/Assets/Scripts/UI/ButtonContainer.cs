using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
	/// <summary>
	/// Container that holds references to objects pertaining to a UI button.
	/// </summary>
	public class ButtonContainer : MonoBehaviour
	{
		/// <summary>
		/// Graphic of the button.
		/// </summary>
		[NotNull]
		public Graphic Graphic => graphic;
		
		/// <summary>
		/// Text on the button.
		/// </summary>
		[NotNull]
		public TextMeshProUGUI TextMesh => textMesh;
		
		/// <summary>
		/// The button component itself.
		/// </summary>
		[NotNull]
		public Button Button => button;
		
		[SerializeField] private Graphic graphic;
		[FormerlySerializedAs("text")] [SerializeField] private TextMeshProUGUI textMesh;
		[SerializeField] private Button button;

		/// <summary>
		/// Add a function that is called when the button is pressed.
		/// </summary>
		/// <param name="listener">Function that is called when the button is pressed.</param>
		public void AddListener(UnityAction listener)
		{
			button.onClick.AddListener(listener);
		}

		/// <summary>
		/// Remove all functions that are listening to the button click.
		/// </summary>
		public void RemoveListeners()
		{
			button.onClick.RemoveAllListeners();
		}
		
	}
}