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
	}
}