using TMPro;
using UnityEngine;

namespace UI
{
	/// <summary>
	/// Controls a generic UI icon like the health under a minion.
	/// </summary>
	public class IconController : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI iconText;

		/// <summary>
		/// Set the text of the icon.
		/// </summary>
		/// <param name="text">Text to put on the icon.</param>
		public void SetText(string text)
		{
			iconText.text = text;
		}

		/// <summary>
		/// Determines whether the icon is shown or not.
		/// </summary>
		/// <param name="show">Whether to show or not.</param>
		public void Show(bool show)
		{
			gameObject.SetActive(show);
		}
	}
}