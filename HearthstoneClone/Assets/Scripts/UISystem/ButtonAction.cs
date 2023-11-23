using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
	[RequireComponent(typeof(Button))]
	public abstract class ButtonAction : MonoBehaviour
	{
		protected Button Button;
		
		protected virtual void Awake()
		{
			Button = GetComponent<Button>();
			Button.onClick.AddListener(OnClick);
		}

		protected abstract void OnClick();
	}
}