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
		
		public void SetManaText(int amount, int maxAmount)
		{
			manaText.text = $"{amount}/{maxAmount} Mana";
		}
	}
}