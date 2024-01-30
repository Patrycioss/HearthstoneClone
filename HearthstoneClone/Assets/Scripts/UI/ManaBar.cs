using TMPro;
using UnityEngine;

namespace UI
{
	/// <summary>
	/// Controls a mana bar in the game.
	/// </summary>
	public class ManaBar : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI manaText;
		
		/// <summary>
		/// Set the mana displayed.
		/// </summary>
		/// <param name="amount">Amount of mana the player currently has.</param>
		public void SetManaText(int amount)
		{
			manaText.text = $"{amount} Mana";
		}
	}
}