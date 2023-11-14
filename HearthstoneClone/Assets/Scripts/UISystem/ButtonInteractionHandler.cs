using Actions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UISystem
{
	[RequireComponent(typeof(Button))]
	public class ButtonInteractionHandler : MonoBehaviour
	{
		[FormerlySerializedAs("action")] [FormerlySerializedAs("buttonAction")] [SerializeField] private GameAction gameAction;
		private Button button;
		
		private void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(OnClick);

			if (!gameAction)
			{
				Debug.LogWarning($"{nameof(gameAction)} is not assigned in {nameof(ButtonInteractionHandler)} on object: {name}!");
			}
		}

		private void OnClick()
		{
			gameAction.Act();
		}
	}
}